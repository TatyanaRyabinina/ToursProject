tourproject.login = {
	login: function login(form){
		const objForm = $(form),
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let dataLoginForm = JSON.stringify(objForm.serializeObject());

			objForm.addClass('loading');
			tourproject.ajax.sendPOST("/Account/Login", dataLoginForm)
			.done((response) => {
				if (response && response.status) {
					location.href = location.origin + "/Tour/Index";
				}
				else if (response && !response.status && response.error.length > 0) {
					$("hr").after((`<span class='error text-danger'> ${response.error} </span>`));
					objForm.removeClass('loading');
				}
			})
		}
	},
	register: function register(form) {
		const objForm = $(form),
			isValid = tourproject.validate.validate(objForm);

		if (isValid) {
			let formData = this.collectDataFormRegister(objForm);

			objForm.addClass('loading');
			this.sendRegisterForm(formData)
			.then((response) => {
				response = JSON.parse(response);

				if (response && response.status) {
					location.href = location.origin + "/Tour/Index";
				}
				else if(response && !response.status && response.error.length > 0) {
					$("hr").after((`<span class='error text-danger'> ${response.error} </span>`));
					objForm.removeClass('loading');
				}
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

		formData.append('Photo', ($('#photoFile')[0].files[0]), imageName);
		return formData;
	},
	sendRegisterForm: function sendRegisterForm(data) {
		return new Promise(function (resolve, reject) {
			let request = new XMLHttpRequest();

			request.open("POST", "/Account/Register");
			request.onload = function() {
				resolve(this.responseText);
			};

			request.send(data);
		});
	}
};
