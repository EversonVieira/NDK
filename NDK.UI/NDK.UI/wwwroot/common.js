export function setInputValue(element, value) {

    element.value = value;
    
}

export function getInputValue(element) {
    if (!element || !element.value) {
        return null;
    };

    return element.value;

}
