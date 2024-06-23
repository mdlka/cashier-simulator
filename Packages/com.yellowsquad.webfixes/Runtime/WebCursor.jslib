const library = {
    
    // Class definition.

    $webCursor: {
        initialize: function (onPointerLockChangeCallbackPtr) {
            const canvas = Module["canvas"];

            dynCall('vi', onPointerLockChangeCallbackPtr, [document.pointerLockElement == canvas]);
            document.addEventListener("pointerlockchange", function() {
                dynCall('vi', onPointerLockChangeCallbackPtr, [document.pointerLockElement == canvas]);
            });
        },

        requestPointerLock: function () {
            canvas.requestPointerLock();
        },
    },

    // External C# calls.

    WebCursorInitialize: function (onPointerLockChangeCallbackPtr) {
        webCursor.initialize(onPointerLockChangeCallbackPtr);
    },

    RequestPointerLock: function () {
        webCursor.requestPointerLock();
    },
}

autoAddDeps(library, '$webCursor');
mergeInto(LibraryManager.library, library);
