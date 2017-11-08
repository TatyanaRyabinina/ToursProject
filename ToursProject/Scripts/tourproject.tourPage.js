tourproject.tourPage = {
	init: function init() {
		const isToursList = location.hash === "#toursList" ? true : false;

		if(isToursList){
			this.loadToursListContent();
		}

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
		$(document).on("keyup", "input.formExcursions", (e) => {
			const form = $(e.currentTarget.form);

			form.find(".excursionSightEle").remove();
		});
		$(document).on("keyup", "input.addExcursionSight", (e) => {
			if (e.keyCode == 13) { //if enter was pressed
				let excursionSightEle = $(e.currentTarget),
					excursionSightValue = excursionSightEle.val(),
					valid = true,
					inputValid;

				if (excursionSightValue.length === 0) {
					valid = false;
				}

				inputValid = tourproject.validate.validateInput(excursionSightEle);
				if (!inputValid) {
					valid = false;
				}

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
		tourproject.ajax.sendRequest("/Tour/EditTour", `id=${ids}`, "GET")
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
	closeTourForm: function closeTourForm(form) {
		const objForm = $(form),
			popup = $(form.parentElement);

		objForm.trigger("reset");
		objForm.find(".excursionSightEle").remove();
		popup.dialog("close");
		alert("Tour Form was closed without saving data.")
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
		return tourproject.ajax.sendRequest(url, null, "GET")
			.then((response) => {
				let responseData = [];

				if (response && response.status === "success") {
					responseData = response.result.Data.selectedData;
				}
				this.setAutocomplete(valueEle, responseData);

				return responseData;
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
		const excursionSight = $(".excursionSight"),
			excursionSightDiv = excursionSight.parent();

		excursionSight.append(`<li class="excursionSightEle">${excursionSightValue} <input type="button" value="Remove" onclick="tourproject.tourPage.removeExcursionSight()"/></li>`);
		$(".addExcursionSight").val("");
		excursionSightDiv.find(".error").remove();
	},
	removeExcursionSight: function removeExcursionSight() {
		let currentExcursionSight = event.currentTarget,
			excursionSightEle = currentExcursionSight.parentElement;

		excursionSightEle.remove();
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
			tourproject.ajax.sendRequest("/Tour/AddTour", JSON.stringify(dataAddTourForm), "POST")
			.done((response) => {
				if (response && response.status) {
					objForm.trigger("reset");
					objForm.find(".excursionSightEle").remove();

					this.submitSucsess(objForm, "added");
				} else if (response && !response.status && response.error.length > 0) {
					tourproject.validate.applyFieldError($("#errorAddForm"), response.error, "append");
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
			tourproject.ajax.sendRequest("/Tour/Edit", JSON.stringify(dataEditTourForm), "POST")
			.done((response) => {
				if (response && response.status) {
					this.submitSucsess(objForm, "edited");
				} else if (response && !response.status && response.error.length > 0) {
					tourproject.validate.applyFieldError($("#errorEditForm"), response.error, "append");
					objForm.removeClass("loading");
				}
			});
		}
	},
	submitSucsess: function submitSucsess(objForm, method) {
		objForm.removeClass("loading");
		objForm.parent().dialog("close");

		$("#list").trigger("reloadGrid");

		setTimeout(() => {
			alert(`Tour was ${method}!`);
		}, 100);
	},
	loadToursListContent: function loadToursListContent() {
		tourproject.ajax.sendRequest("/Tour/Index", null, "GET")
		.done((response) => {
			if (response) {
				$("body").html(response);
				this.renderToursList();
			}
		});
	}
};
