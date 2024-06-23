const library = {
    
    // Class definition.

    $webMouse: {
        requestPointerLock: function () {
            canvas.requestPointerLock();
        },
    },

    // External C# calls.

    RequestPointerLock: function () {
        webMouse.requestPointerLock();
    },
}

autoAddDeps(library, '$webMouse');
mergeInto(LibraryManager.library, library);
