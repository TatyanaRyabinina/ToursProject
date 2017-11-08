tourproject.init = () => {
	tourproject.init.loadContent = () => {
		const isLogged = localStorage.getItem("isLogged");

		if (isLogged && location.hash === "#toursList") {
			tourproject.tourPage.init();
			return;
		}
		tourproject.login.init();
	};

	tourproject.init.loadContent();

	$(window).on("hashchange", () => {
		if (tourproject.init.loadContent) {
			tourproject.init.loadContent();
		}
	});
};