tourproject.tourPage = {
	init: function init() {
		this.renderToursList();
		$(".excursionSight").sortable();

		$(document).on("focus", "input.addExcursionSight", (e) => {
			this.excursionSights = [];
			const form = $(e.currentTarget.form),
				excursionSightEle = $(e.currentTarget),
				excursionEle = form.find(".formExcursions"),
				excursionValue = excursionEle.val();
			let url;

			if (excursionValue && excursionValue.length > 0) {
				url = `/Tour/GetAllExcursionSights?excursionValue=${excursionValue}`;
				this.getAutocompleteInfo(excursionSightEle, url)
				.done((response) => {
					if (response) {
						this.excursionSights = response;
					}
				});
			} else {
				this.setAutocomplete(excursionSightEle, this.excursionSights);
			}
		});
		$(document).on("keyup", "input.addExcursionSight", (e) => {
			if (e.keyCode == 13) {
				let excursionSightEle = $(e.currentTarget),
					excursionSightValue = excursionSightEle.val(),
					valid = true;

				valid = tourproject.validate.validateInput(excursionSightEle);
				if (valid) {
					this.addExcursionSight(excursionSightValue);
				}
			}
		});
	},
	renderToursList: function renderToursList() {
		$("#list").jqGrid({
			width: 650,
			height: 300,
			url: "/Tour/GetTours",
			datatype: "json",
			mtype: "POST",
			jsonReader: {
				page: "page",
				total: "total",
				records: "records",
				root: "rows",
				repeatitems: false,
				id: ""
			},
			colNames: ["#", "Date", "Client", "Excursion"],
			colModel: [
				{ name: "OrderedTourId", index: "OrderedTourId", key: true, width: 75, editable: true },
				{ name: "Date", index: "Date", width: 150, editable: true },
				{ name: "ClientName", index: "ClientName", width: 200, editable: true },
				{ name: "ExcursionName", index: "ExcursionName", width: 200, editable: true },
			],
			rowNum: 5,
			prmNames: { id: "OrderedTourId" },
			rowList: [5, 10, 15],
			pager: "#pager",
			viewrecords: true,
			viewsortcols: [false, false, false, false],
			onSelectRow: (id) => {
				this.editTourDialogOpen(id);
			}
		})
	},
	addTourDialogOpen: function addTourDialogOpen() {
		$("#AddTour").dialog({
			width: 600,
			title: "Add New Tour",
			closeText: "",
			open: (() => {
				let form = $("#AddTourForm"),
					errorMessage = form.find(".error");

				if (errorMessage.length > 0) {
					errorMessage.remove();
				}
				this.collectAutocompleteObject(form);
			})
		})
	},
	editTourDialogOpen: function editTourDialogOpen(ids) {
		$.getJSON("/Tour/EditTour?id=" + ids)
		.done((response) => {
			$("#EditTour").html(response);
			$("#EditTour").dialog({
				width: 600,
				title: "Edit Tour",
				closeText: "",
				open: (() => {
					let form = $("#EditTourForm");
					this.collectAutocompleteObject(form);
					$(".excursionSight").sortable();
				})
			})
		});
	},
	collectAutocompleteObject: function collectAutocompleteObject(form) {
		let valueEleExcursion = form.find(".formExcursions"),
			valueEleClient = form.find(".formClients");

		$.when(
			this.getAutocompleteInfo(valueEleExcursion, "/Tour/GetAllExcursions"),
			this.getAutocompleteInfo(valueEleClient, "/Tour/GetAllClients")
		).then((responseExcursion, responseClient) => {
			if (responseExcursion) {
				this.availableExcursions = responseExcursion;
			}
			if (responseClient) {
				this.availableClients = responseClient;
			}
		});
	},
	getAutocompleteInfo: function getAutocompleteInfo(valueEle, url) {
		return $.getJSON(url)
			.then((response) => {
				let availableData = [];
				if (response && response.status === "success") {
					let responseData = response.result.Data.selectedData;

					availableData = responseData;
				}
				this.setAutocomplete(valueEle, availableData);

				return availableData;
			});
	},
	setAutocomplete: function setAutocomplete(elem, source) {
		elem.autocomplete({
			source: source,
			select: (event, ui) => {
				elem.val(ui.item.value);
				if (elem.hasClass("addExcursionSight")) {
					this.addExcursionSight(ui.item.value);
					event.preventDefault();
				}
			}
		});
	},
	addExcursionSight: function addExcursionSight(excursionSightValue) {
		let excursionSight = $('.excursionSight'),
		excursionSightDiv = excursionSight.parent();

		excursionSight.append(`<li class="excursionSightEle">${excursionSightValue} <input type="button" value="Remove" onclick="tourproject.tourPage.removeExcursionSight()"/></li>`);
		$(".addExcursionSight").val("");
		excursionSightDiv.find(".error").remove();
	},
	removeExcursionSight: function removeExcursionSight() {
		let currentExcursionSight = event.currentTarget,
			excursionSightEle = currentExcursionSight.parentElement,
			excursionSightValue = $.trim(excursionSightEle.innerText),
			objForm = $(currentExcursionSight.form),
			excursionEle = objForm.find(".formExcursions"),
			excursionValue = excursionEle.val();

		tourproject.ajax.sendPOST("/Tour/DeleteExcursionSight", JSON.stringify({ ExcursionSightName: excursionSightValue, ExcursionName: excursionValue }))
		.done((response) => {
			if (response && response.status) {
				excursionSightEle.remove();
			}
		})
	},
	toggleDateTime: function toggleDateTime(form) {
		const datepicker = $(form).find(".datepicker");

		if (!datepicker.hasClass("datepicker-opened")) {
			datepicker.datepicker();
			datepicker.datepicker("show");
			datepicker.addClass("datepicker-opened");
		} else {
			datepicker.datepicker("hide");
			datepicker.removeClass("datepicker-opened");
		}
	},
	collectDataTourForm: function collectDataTourForm(form) {
		let dataTourForm = form.serializeObject(),
			excursionSight = [],
		excursionSightEle = form.find(".excursionSightEle"),
		excursionSightLength = excursionSightEle.length;

		dataTourForm.Date = new Date(dataTourForm.Date);

		for (let i = 0; i < excursionSightLength; i++) {
			excursionSight.push($.trim(excursionSightEle[i].innerText))
		}
		dataTourForm.ExcursionSight = excursionSight;

		return dataTourForm;
	},
	submitAddTourForm: function submitAddTourForm(form) {
		const objForm = $(form);
		let data = {},
			addExcursionSight = objForm.find(".addExcursionSight"),
			addExcursionSightVal = addExcursionSight.val(),
			isValid;

		if (addExcursionSightVal) {
			this.addExcursionSight(addExcursionSightVal);
		}
		isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			dataAddTourForm = this.collectDataTourForm(objForm);

			objForm.addClass("loading");
			tourproject.ajax.sendPOST("/Tour/AddTour", JSON.stringify(dataAddTourForm))
			.done((response) => {
				if (response && response.status) {
					objForm.removeClass("loading");
					objForm.trigger("reset");
					objForm.find(".excursionSightEle").remove();
					$("#AddTour").dialog("close");
					$("#list").trigger("reloadGrid");
					alert("New Tour was added!");
				} else if (response && !response.status && response.error.length > 0) {
					$("#errorAddForm").append((`<span class="error text-danger"> ${response.error} </span>`));
					objForm.removeClass("loading");
				}
			});
		}
	},
	submitEditTourForm: function submiEditTourForm(form) {
		const objForm = $(form);
		let data = {},
			addExcursionSight = objForm.find(".addExcursionSight"),
			addExcursionSightVal = addExcursionSight.val(),
			isValid;

		if (addExcursionSightVal) {
			this.addExcursionSight(addExcursionSightVal);
		}
		isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			dataEditTourForm = this.collectDataTourForm(objForm);

			objForm.addClass("loading");
			tourproject.ajax.sendPOST("/Tour/Edit", JSON.stringify(dataEditTourForm))
			.done((response) => {
				if (response && response.status) {
					objForm.removeClass("loading");

					$("#EditTour").dialog("close");
					$("#list").trigger("reloadGrid");
					alert("Tour was edited!");
				} else if (response && !response.status && response.error.length > 0) {
					$("errorEditForm").append((`<span class="error text-danger"> ${response.error} </span>`));
					objForm.removeClass("loading");
				}
			});
		}
	}
};
