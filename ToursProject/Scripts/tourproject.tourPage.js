tourproject.tourPage = {
	init: function init() {
		this.renderToursList();
	},
	renderToursList: function renderToursList() {
		$("#list").jqGrid({
			width: 650,
			height: 300,
			url: "/Tour/GetTours",
			datatype: 'json',
			mtype: 'POST',
			jsonReader: {
				page: "page",
				total: "total",
				records: "records",
				root: "rows",
				repeatitems: false,
				id: ""
			},
			colNames: ['#', 'Date', 'Client', 'Excursion'],
			colModel: [
				{ name: 'OrderedTourId', index: 'OrderedTourId', key: true, width: 75, editable: true },
				{ name: 'Date', index: 'Date', width: 150, editable: true },
				{ name: 'ClientName', index: 'ClientName', width: 200, editable: true },
				{ name: 'ExcursionName', index: 'ExcursionName', width: 200, editable: true },
			],
			rowNum: 10,
			prmNames: { id: "OrderedTourId" },
			rowList: [10, 20, 30],
			pager: '#pager',
			viewrecords: true,
			viewsortcols: [false, false, false, false],
			onSelectRow: function () {
			}
		});
	},
	addTourDialogOpen: function addTourDialogOpen() {
		$("#AddTour").dialog({
			width: 400,
			title: "Add New Tour",
			closeText: ""
		});
		$.when(
			$.getJSON("/Tour/GetAllExcursions"),
			$.getJSON("/Tour/GetAllClients")
		).done((responseExcursions, responseClients) => {
			if (responseExcursions && responseExcursions[1] === "success") {
				let availableExcursions = [];

				for (let x in responseExcursions[0].Data.allExcursions) {
					availableExcursions.push({ value: responseExcursions[0].Data.allExcursions[x].ExcursionName, id: responseExcursions[0].Data.allExcursions[x].ExcursionId });
				}
				this.setAutocomplete($("#excursions"), availableExcursions, $("#excursionId"));
			}
			if (responseClients && responseClients[1] === "success") {
				let availableClients = [];

				for (let x in responseClients[0].Data.allClients) {
					availableClients.push({ value: responseClients[0].Data.allClients[x].ClientName, id: responseClients[0].Data.allClients[x].ClientId });
				}
				this.setAutocomplete($("#clients"), availableClients, $("#clientId"));
			}
		});
		$(document).on("keyup ", "input", (e) => {
			let input = $(e.currentTarget),
				editorfor = input.attr("data-editorfor");
			$(editorfor).val("");
		});
	},
	setAutocomplete: function setAutocomplete(elem, source, elemId) {
		elem.autocomplete({
			source: source,
			select: (event, ui) => {
				elem.val(ui.item.value);
				elemId.val(ui.item.id);
			}
		});
	},
	toggleDateTime: function toggleDateTime() {
		const datepicker = $("#datepicker");

		if (!datepicker.hasClass('datepicker-opened')) {
			datepicker.datepicker();
			datepicker.datepicker('show');
			datepicker.addClass('datepicker-opened');
		} else {
			datepicker.datepicker('hide');
			datepicker.removeClass('datepicker-opened');
		}
	},
	submitAddTourForm: function submitAddTourForm(form) {
		const objForm = $(form);
		let data = {},
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let dataAddTourForm = JSON.stringify(objForm.serializeObject());

			objForm.addClass('loading');
			tourproject.ajax.sendPOST("/Tour/AddTourForm", dataAddTourForm)
		}
	}
};