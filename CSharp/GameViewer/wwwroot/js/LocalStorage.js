(function () {
	"use strict";

	const Namespace = "GameViewer.JSInteroperability";
	const Class = "LocalStorage";

	const $GameViewer = window.GameViewer;

	if ($GameViewer == undefined)
		throw new Error("GameViewer library not found!");

	const dataTypes = $GameViewer.DataTypes;

	class LocalStorage {
		constructor() {
			throw new $GameViewer.CannotInstantiateStaticClassException(Class);
		}

		static Get(key) {
			if (dataTypes.Is(typeof (Storage), dataTypes.Undefined))
				throw new StorageUnsupportedException();

			return localStorage.getItem(key);
		}

		static Set(key, value) {
			if (dataTypes.Is(typeof (Storage), dataTypes.Undefined))
				throw new StorageUnsupportedException();

			localStorage.setItem(key, value);
		}

		static Remove(key) {
			if (dataTypes.Is(typeof (Storage), dataTypes.Undefined))
				throw new StorageUnsupportedException();

			localStorage.removeItem(key);
		}
	}

	class StorageUnsupportedException
		extends Error {
		constructor() {
			super("Local storage is not supported.");
		}
	}

	$GameViewer.BindToNamespace(
		Namespace,
		Class,
		LocalStorage
	);
})();