tourproject.validate = {
	validate: function validate(form) {
		const inputs = form.find(".text-box"),
			list = form.find("ul"),
			files = form.find(".photoFile");
		let valid = true,
			inputValid,
			passwordValid,
			listValid;

		form.find(".error").remove();
		inputs.each((i, e) => {
			inputValid = this.validateInput(e);
			if (!inputValid) {
				valid = false;
			}
		});

		files.each((i, e) => {
			inputValid = this.validateImage(e);
			if (!inputValid) {
				valid = false;
			}
		});

		listValid = this.validateList(list);
		if (!listValid) {
			valid = false;
		}

		passwordValid = this.passwordsMatch(form);
		if (!passwordValid) {
			valid = false;
		}
		return valid;
	},
	validateInput: function validateInput(formElement) {
		let input = $(formElement),
			val = input.val(),
			maxLength = input.attr("max-length"),
			required = input.attr("data-val-required"),
			validMessage = input.attr("data-val-valid"),
			pattern = input.attr("data-type"),
			regExpMap = tourproject.regExp,
			valid = true,
			validateErrorMessage = "";

		if (required && (!val || val.length === 0)) {
			valid = false;
			validateErrorMessage = required;
		} else if (val && regExpMap[pattern]) {
			regExp = regExpMap[pattern];
			if (regExp && regExp.test) {
				valid = regExp.test($.trim(val));
				if (!valid) {
					validateErrorMessage = `Not valid! ${validMessage ? validMessage : ""}`;
				}
			}
		}
		input.parent().find(".error").remove();
		if (!valid) {
			this.applyFieldError(input, validateErrorMessage, "after");
		}
		return valid;
	},
	validateImage: function validateFile(file) {
		const objFile = $(file),
			image = objFile[0] ? objFile[0].files[0] : {},
			pattern = objFile.attr("data-type") || "",
			required = objFile.attr("data-val-required") || "",
			isEmpty = $.isEmptyObject(image),
			regExpMap = tourproject.regExp;
		let validateErrorMessage = "",
			valid = true;

		if (required && isEmpty) {
			valid = false;
			validateErrorMessage = required;
		}
		if (!isEmpty && (image.size > 4 * 1024 * 1024)) {
			valid = false;
			validateErrorMessage = "The size of image is too large. Please make sure the file is not more than 4 Mb";
		}
		if (!isEmpty && regExpMap[pattern]) {
			regExp = regExpMap[pattern];
			if (regExp && regExp.test) {
				validRegExp = regExp.test(image.type);
				if (!validRegExp) {
					valid = false;
					validateErrorMessage = "File not supported!";
				}
			}
		}
		if (!valid) {
			this.applyFieldError(objFile, validateErrorMessage, "after");
		}
		return valid;
	},
	passwordsMatch: function passwordsMatch(form) {
		const password = form.find(".password"),
			confirmPassword = form.find(".confirmPassword");
		let valid = true,
			validateErrorMessage = "";

		if (password.length > 0 && confirmPassword.length > 0 && $.trim(password.val()) != $.trim(confirmPassword.val())) {
			validateErrorMessage = "Passwords don't match!";
			valid = false;
		}
		if (!valid) {
			this.applyFieldError(confirmPassword, validateErrorMessage, "after");
		}
		return valid;
	},
	validateList: function validateList(list) {
		let listEle = list.find("li"),
			validateErrorMessage ="",
			valid = true;

		if (list.length > 0 && listEle.length === 0) {
			valid = false;
			validateErrorMessage = "At least one item is required!";
		}
		if(!valid){
			this.applyFieldError(list, validateErrorMessage, "after");
		}
		return valid;
	},
	applyFieldError: function applyFieldError(ele, validateErrorMessage, method) {
		ele[method](`<span class="error text-danger"> ${validateErrorMessage} </span>`);
	}
};

tourproject.regExp = {
	email: /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/,
	name: /^[a-zA-Zа-яА-ЯёЁ-]{1,20}$/,
	password: /^[a-zA-Z0-9._%+-,`!?<>:;''""@#$^&*()/=]{6,}$/,
	photo: /image.(jpg|jpeg|gif|png|ico)/i,
	date: /(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/](20)\d\d$/,
	clientname: /^[a-zA-Zа-яА-ЯёЁ-]{1,20}[ ][a-zA-Zа-яА-ЯёЁ-]{1,20}$/,
	excursionname: /^[a-zA-Zа-яА-ЯёЁ-]{1,15}[ -]?[a-zA-Zа-яА-ЯёЁ-]{1,15}[ -]?[a-zA-Zа-яА-ЯёЁ-]{1,15}$/
};
