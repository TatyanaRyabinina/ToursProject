tourproject.login = {
	login: function login(form) {
		const objForm = $(form),
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let dataLoginForm = JSON.stringify(objForm.serializeObject());

			objForm.addClass("loading");
			tourproject.ajax.sendRequest("/Account/Login", dataLoginForm, "POST")
			.done((response) => {
				this.doneSubmit(response, objForm);
			})
		}
	},
	register: function register(form) {
		const objForm = $(form),
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let formData = this.collectDataFormRegister(objForm);

			objForm.addClass("loading");
			this.sendRegisterForm(formData)
			.done((response) => {
				response = JSON.parse(response);

				this.doneSubmit(response, objForm);
			})
		}
	},
	collectDataFormRegister: function collectDataFormRegister(objForm) {
		let formData = new FormData();
		const inputs = objForm.find(".text-box"),
			imageName = $("#Email").val() + ".png";

		inputs.each((i, e) => {
			formData.append(e.getAttribute("name"), e.value);
		});

		formData.append("Photo", ($("#photoFile")[0].files[0]), imageName);

		return formData;
	},
	sendRegisterForm: function sendRegisterForm(data) {
		return new Promise(function (resolve, reject) {
			let request = new XMLHttpRequest();

			request.open("POST", "/Account/Register");
			request.onload = function () {
				resolve(this.responseText);
			};

			request.send(data);
		});
	},
	doneSubmit: function doneSubmit(response, objForm) {
		if (response && response.status) {
			location.href = location.origin + "/Tour/Index";
			return;
		}
		if (response && !response.status && response.error.length > 0) {
			tourproject.validate.applyFieldError($("hr"), response.error);
			objForm.removeClass("loading");
		}
	}
};
