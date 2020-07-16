window.dotNetObject;
window.trainJsFunctions = {
    generatorDotnetObject: function (_dotNetObject) {
        window.dotNetObject = _dotNetObject;
    },
    fixOptionValue: function (option) { option.setAttribute('value', ''); }
};
