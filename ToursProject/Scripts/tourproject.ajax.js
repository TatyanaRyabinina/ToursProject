tourproject.ajax = {
	sendRequest: function sendRequest(url, data, type) {
		return $.ajax({
			url: url,
			type: type,
			data: data,
			contentType: "application/json; charset=utf-8",
			dataType: "json"
		});
	}
};
