tourproject.validate = {
	validate: function validate(form) {
		const inputs = form.find(".text-box"),
			file = $('#photoFile');
		let valid = true,
			inputValid;

		form.find(".error").remove();
		inputs.each((i, e) => {
			inputValid = this.validateInput(e);
			if (!inputValid) {
				valid = false;
			}
		});

		inputValid = this.validateImage(file);
		if (!inputValid) {
			valid = false;
		}
		return valid;
	},
	validateInput: function validateInput(formElement) {
		let input = $(formElement),
			val = input.val(),
			maxLength = input.attr("max-length"),
			required = input.attr("data-val-required"),
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
				valid = regExp.test(val);
				if (!valid) {
					validateErrorMessage = "Not valid!";
				}
			}
		}
		input.parent().find(".error").remove();
		if (!valid) {
			input.after(`<span class='error text-danger'> ${validateErrorMessage} </span>`);
		}
		return valid;
	},

	validateImage: function validateFile(file) {
		const image = file[0] ? file[0].files[0] : {},
			pattern = file.attr("data-type") || "",
			required = file.attr("data-val-required") || "",
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
				valid = regExp.test(image.type);
				if (!valid) {
					validateErrorMessage = "File not supported!";
				}
			}
		}
		if (!valid) {
			file.after(`<span class='error text-danger'> ${validateErrorMessage} </span>`);
		}
		return valid;
	}
};

tourproject.regExp = {
	email: /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/,
	name: /^[a-zA-Zа-яА-ЯёЁ-]{1,20}$/,
	password: /^[a-zA-Z0-9._%+-,`!?<>:;''""@#$^&*()/=]{6,}$/,
	photo: /image.(jpg|jpeg|gif|png|ico)/i,
	date: /(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/](20)\d\d$/,
	clientname: /^[a-zA-Zа-яА-ЯёЁ-]{1,20}[ ][a-zA-Zа-яА-ЯёЁ-]{1,20}$/,
	excursionname: /^[a-zA-Zа-яА-ЯёЁ-]{1,15}[ -]?[a-zA-Zа-яА-ЯёЁ-]{1,15}[ -]?[a-zA-Zа-яА-ЯёЁ-]{1,15}?$/
};
