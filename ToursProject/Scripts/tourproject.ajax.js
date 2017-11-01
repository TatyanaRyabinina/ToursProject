tourproject.ajax = {
	sendPOST: function sendPOST(url, data) {
		return $.ajax({
			url: url,
			type: "POST",
			data: data,
			contentType: "application/json; charset=utf-8",
			dataType: "json"
		});
	}
};
