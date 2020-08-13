(function () {
	"use strict";

	const Namespace_GameViewer = "GameViewer";

	function BindToNamespace(
		namespace,
		objName,
		obj,
		overwrite = false
	) {
		if (typeof (namespace) !== typeof (""))
			throw exceptions.UnexpectedDataType("namespace");

		if (typeof (objName) !== typeof (""))
			throw exceptions.UnexpectedDataType("objName");

		if (
			typeof (overwrite) !== typeof (false)
			&& typeof (overwrite) !== typeof (undefined))
			throw exceptions.UnexpectedDataType("overwrite");

		var namespaces = String(namespace)
			.split(".")
			;

		var currentNamespace = window
		while (namespaces.length > 0) {
			var name = namespaces.shift();

			if (typeof (currentNamespace[name]) === typeof (undefined))
				Object.defineProperty(currentNamespace, name, {
					value: {},
					writable: true,
					configurable: true,
					enumerable: true
				});
			currentNamespace = currentNamespace[name];
		}

		if (
			!overwrite
			&& currentNamespace[objName] !== undefined
		)
			throw new Error("Cannot overwrite namespace when overwrite is set to false.");

		Object.defineProperty(
			currentNamespace,
			objName,
			{
				value: obj,
				writable: true,
				configurable: true,
				enumerable: true
			}
		);
	};

	BindToNamespace(
		Namespace_GameViewer,
		"BindToNamespace",
		BindToNamespace
	);

	(function () {
		class BaseClass {
			static StaticConstructorExecuted
				= false;

			constructor() {
				this
					.constructor
					.StaticConstructor()
					;
			}

			static StaticConstructor() {

			}
		}
		BindToNamespace(
			Namespace_GameViewer,
			"BaseClass",
			BaseClass
		);
	})();

	(function () {
		class KeyValuePair {
			constructor(
				key,
				value
			) {
				if (GameViewer.DataTypes.Is(key, GameViewer.DataTypes.Undefined))
					throw GameViewer.Exceptions.ArgumentNullException()

				this._key = key;
				this._value = value;
			}

			get Key() {
				return this._key;
			}

			get Value() {
				return this._value;
			}
		}
		BindToNamespace(
			Namespace_GameViewer,
			"KeyValuePair",
			KeyValuePair
		);
	})();

	(function () {
		class Arguments {
			constructor() {
				throw new CannotInstantiateStaticClassException("CustomFormatSpecifiers");
			}

			static ToArray(
				args,
				start = undefined,
				end = undefined
			) {
				if (!(args instanceof arguments))
					throw new UnexpectedDataTypeException("args");

				if (!DataTypes.IsAny(
					start,
					[
						DataTypes.Number,
						DataTypes.Undefined
					]
				))
					throw new UnexpectedDataTypeException("start");

				if (!DataTypes.IsAny(
					end,
					[
						DataTypes.Number,
						DataTypes.Undefined
					]
				))
					throw new UnexpectedDataTypeException("end");

				return Array
					.prototype
					.slice
					.call(args, start, end)
					;
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"Arguments",
			Arguments
		);
	})();

	(function () {
		class DataTypes {
			constructor() {
				throw new CannotInstantiateStaticClassException("DataTypes");
			}

			static Is(obj, type) {
				return (typeof obj) === (typeof type);
			}

			static IsAny(obj, types) {
				if (!dataTypesObject.IsArray(types))
					throw exceptions.UnexpectedDataType("types");

				for (var i = 0; i < types.length; i++)
					if (dataTypesObject.Is(obj, types[i]))
						return true;

				return false;
			}

			static IsArray(obj) {
				return Array.isArray(obj);
			}

			static TypeOf() {
				return {
					String: typeof "",
					Number: typeof 0,
					Boolean: typeof false,
					Function: typeof function () { },
					Object: typeof {},
					Undefined: typeof undefined
				};
			}

			static get String() {
				return "";
			}

			static get Number() {
				return 0;
			}

			static get Boolean() {
				return false;
			}

			static get Function() {
				return function () { };
			}

			static get Object() {
				return {};
			}

			static get Undefined() {
				return undefined;
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"DataTypes",
			DataTypes
		);
	})();

	(function () {
		class ArgumentNullException
			extends Error {
			constructor(argumentName) {
				if (!DataTypes.Is(argumentName, DataTypes.String))
					throw new UnexpectedDataTypeException("argumentName");

				super("Argument null - \"" + argumentName + "\".");
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"ArgumentNullException",
			ArgumentNullException
		);
	})();

	(function () {
		class InvalidOperationException
			extends Error {
			constructor(message) {
				if (!DataTypes.Is(message, DataTypes.String))
					throw new UnexpectedDataTypeException("message");

				super(message);
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"InvalidOperationException",
			InvalidOperationException
		);
	})();

	(function () {
		class UnexpectedDataTypeException
			extends Error {
			constructor(argumentName) {
				if (!DataTypes.Is(argumentName, DataTypes.String))
					throw new UnexpectedDataTypeException("message");

				super("Unexpected data type - \"" + argumentName + "\".");
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"UnexpectedDataTypeException",
			UnexpectedDataTypeException
		);
	})();

	(function () {
		class CannotInstantiateStaticClassException
			extends Error {
			constructor(staticClassName) {
				if (!DataTypes.Is(staticClassName, DataTypes.String))
					throw new UnexpectedDataTypeException("staticClassName");

				super("Cannot instantiate a static class - \"" + argumentName + "\".");
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"CannotInstantiateStaticClassException",
			CannotInstantiateStaticClassException
		);
	})();

	(function () {
		var dataTypes = GameViewer.DataTypes;

		class Obj {
			constructor() {
				throw new GameViewer.CannotInstantiateStaticClassException("Obj");
			}

			static GetAllKeys(obj) {
				if (dataTypes.Is(obj, dataTypes.Undefined))
					throw new UnexpectedDataTypeException("obj");

				var keys = [];

				for (var key in obj)
					keys.push(key);

				return keys;
			}

			//	Converts an object of any type into a plain old CRL object.
			//	[Plain old CLR object](https://en.wikipedia.org/wiki/Plain_old_CLR_object)
			//
			//	Some objects based on how they are structured may have self-referencing recursion
			//	when trying to run an operation that recursively traverses an object and its
			//	descendents.
			static ToPoco(
				obj,
				maximumDepth = 64,
				ignoreSelfReferencing = false
			) {
				if (dataTypes.Is(obj, dataTypes.Undefined))
					throw new UnexpectedDataTypeException("obj");

				var rootPoco = new GameViewer.KeyValuePair(
					new GameViewer.KeyValuePair(
						obj,
						{}
					),
					this.GetAllKeys(obj)
				);

				var depth = [rootPoco];
				while (depth.length > 0) {
					var kvp = depth.pop();

					//	Build properties.
					for (var properties = kvp.Value; properties.length > 0; properties.shift()) {
						var k = kvp.Key;
						var o = k.Key[properties[0]]

						if (dataTypes.Is(o, dataTypes.Object)) {
							//	This value can be evaluated as a POCO.
							kvp = new GameViewer.KeyValuePair(
								new GameViewer.KeyValuePair(
									o,
									{}
								),
								this.GetAllKeys(o)
							);

							//	Determine if maximum depth is not hit.
							if (maximumDepth-- > 0) {
								//	Determine if this value was previously evaluated.

								var evaluated = false;
								for (var i = 0; i < depth.length; i++) {
									var depthObj = depth[i]
										.Key
										.Key
										;

									if (o == depthObj) {
										evaluated = true;

										if (!ignoreSelfReferencing)
											//	Reuse previous POCO.
											k.Value[properties[0]] = depth[i]
												.Key
												.Value
												;

										break;
									}
								}

								if (!evaluated) {
									//	Object is has not been evaluated previously.
									//	Add to the depth list and restart.
									depth.push(kvp);

									break;
								}
							}
						}
						else
							k.Value[properties[0]] = o;
					}

					//	Resume, if possible
					if (depth.length > 0) {
						var lastKvp = depth[depth.length - 1];
						var k = lastKvp.Key;
						var properties = lastKvp.Value;

						k.Value[properties[0]] = kvp
							.Key
							.Value
							;

						properties.shift();
					}
				}

				return rootPoco
					.Key
					.Value
					;
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"Object",
			Obj
		);
	})();

	(function () {
		class Number {
			constructor() {
				throw new GameViewer.CannotInstantiateStaticClassException("Number");
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"Number",
			Number
		);
	})();

	(function () {
		class String {
			constructor() {
				throw new GameViewer.CannotInstantiateStaticClassException("String");
			}

			static Repeat(str, count) {
				if (!DataTypes.Is(str, DataTypes.String))
					throw exceptions.UnexpectedDataType("str");

				if (!DataTypes.Is(count, DataTypes.Number))
					throw exceptions.UnexpectedDataType("count");

				var result = "";

				while (count-- > 0)
					result += str;

				return result;
			}
		}

		BindToNamespace(
			Namespace_GameViewer,
			"String",
			String
		);
	})();

	function DefineProperties(
		writable,
		configurable,
		enumerable
	) {
		if (!DataTypes.Is(writable, DataTypes.Boolean))
			throw exceptions.UnexpectedDataType("writable");

		if (!DataTypes.Is(configurable, DataTypes.Boolean))
			throw exceptions.UnexpectedDataType("configurable");

		if (!DataTypes.Is(enumerable, DataTypes.Boolean))
			throw exceptions.UnexpectedDataType("enumerable");

		var valuesArray = argumentsToArray(arguments, 3);

		values = [];
		// for (var value in valuesArray)
	};

	//	Logging
	var logger = (function () {

		return {

		};
	})();

	var events = {};

	Object.defineProperties(
		events,
		(function () {
			const EventInterface = "Event";

			const LoadedEvent = "Loaded";
			const loadedEvent = document.createEvent(EventInterface);
			loadedEvent.initEvent(LoadedEvent, true, false);

			return {
				Loaded: {
					value: loadedEvent,
					writable: false,
					configurable: false,
					enumerable: true
				}
			};
		})()
	);

	var BindToNamespace = function (fqn, obj, overwrite) {
		if (!DataTypes.Is(fqn, DataTypes.String))
			throw exceptions.UnexpectedDataType("fqn");

		if (!DataTypes.IsAny(overwrite, [DataTypes.Boolean, DataTypes.Undefined]))
			throw exceptions.UnexpectedDataType("overwrite");
		if (overwrite === DataTypes.Undefined)
			overwrite = true;

		var namespaces = String(fqn)
			.split(".")
			;

		var currentNamespace = window
		while (namespaces.length > 1) {
			var name = namespaces.shift();

			if (DataTypes.Is(currentNamespace[name], DataTypes.Undefined))
				Object.defineProperty(currentNamespace, name, {
					value: {},
					writable: true,
					configurable: true,
					enumerable: true
				});
			currentNamespace = currentNamespace[name];
		}

		var targetNamespace = [namespaces.shift()]

		if (
			!overwrite
			&& targetNamespace !== DataTypes.Undefined
		)
			throw new Error("Cannot overwrite namespace when overwrite is set to false.");

		Object.defineProperty(currentNamespace, targetNamespace, {
			value: obj,
			writable: true,
			configurable: true,
			enumerable: true
		});
	};
})();
