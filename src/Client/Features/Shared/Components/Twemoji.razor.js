export function updateTwemoji(elementId, text) {
    const element = document.getElementById(elementId);
    
    for (const child of element.childNodes || []) {
        element.removeChild(child);
    }
    element.innerText = text;
    
    twemoji.parse(element, {
        folder: 'svg',
        ext: '.svg',
        className: 'twemoji'
    })
}