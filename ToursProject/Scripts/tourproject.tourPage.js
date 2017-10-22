tourproject.tourPage = {
	init: function init() {
		this.renderToursList();

		$(document).on("focus", "#addExcursionSight", (e) => {
			console.log(this);
			const form = $(e.currentTarget.form),
				excursionSightEle = $(e.currentTarget),
				excursionIdEle = form.find(".formExcursionId"),
				excursionValue = excursionIdEle.val();
			let url;

			if (excursionValue.length > 0) {
				url = `/Tour/GetAllExcursionSights?id=${excursionValue}`;
			}
			this.excursionSights = this.getAutocompleteInfo(excursionSightEle, null, url);
		});

		$(document).on("keyup paste", "input", (e) => {
			let input = $(e.currentTarget),
				editorfor = input.attr("data-editorfor"),
				array;

			if (input.hasClass("formExcursions")) {
				array = this.availableExcursions;
			} else if (input.hasClass("formClients")) {
				array = this.availableClients;
			}

			let selectedValue = $.grep(array, (el) => {
				return el.value.toLowerCase() === input.val().toLowerCase();
			});

			if (!selectedValue || !selectedValue.length) {
				$(editorfor).val("");
			} else {
				$(editorfor).val(selectedValue[0].id);
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
			rowNum: 10,
			prmNames: { id: "OrderedTourId" },
			rowList: [10, 20, 30],
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
					let form = $("#AddTourForm");
					this.collectAutocompleteObject(form);
				})
			})
		});
	},
	collectAutocompleteObject: function collectAutocompleteObject(form) {
		let valueEleExcursion = form.find(".formExcursions"),
			idEleExcursion = form.find(".formExcursionId"),
			valueEleClient = form.find(".formClients"),
			idEleClient = form.find(".formClientId");

		$.when(
			this.getAutocompleteInfo(valueEleExcursion, idEleExcursion, "/Tour/GetAllExcursions"),
			this.getAutocompleteInfo(valueEleClient, idEleClient, "/Tour/GetAllClients")
	).then((responseExcursion, responseClient) => {
		if (responseExcursion)
		this.availableExcursions = responseExcursion;
		this.availableClients = responseClient;
	});
			},
	getAutocompleteInfo: function getAutocompleteInfo(valueEle, idEle, url) {
		return $.getJSON(url)
			.then((response) => {
				if (response && response.status === "success") {
					let responseData = response.result.Data.selectedData;

					this.availableData = responseData;
					this.setAutocomplete(valueEle, responseData, idEle);
				}
				return this.availableData;
			});
	},
	setAutocomplete: function setAutocomplete(elem, source, elemId) {
		elem.autocomplete({
			source: source,
			select: (event, ui) => {
				elem.val(ui.item.value);
				if (elemId) {
					elemId.val(ui.item.id);
				}
			}
		});
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
	submitAddTourForm: function submitAddTourForm(form) {
		const objForm = $(form);
		let data = {},
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let dataAddTourForm = objForm.serializeObject();

			dataAddTourForm.Date = new Date(dataAddTourForm.Date);
			objForm.addClass("loading");
			tourproject.ajax.sendPOST("/Tour/AddTour", JSON.stringify(dataAddTourForm))
			.done((response) => {
				if (response && response.status) {
					objForm.removeClass("loading");
					objForm.trigger("reset");

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
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let dataEditTourForm = objForm.serializeObject();

			dataEditTourForm.Date = new Date(dataEditTourForm.Date);

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
