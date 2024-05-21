const createConverter = () => new showdown.Converter({
    emoji: true,
    parseImgDimensions: true,
    strikethrough: true,
    simplifiedAutoLink: true
});

export function renderMd(md) {
    return createConverter().makeHtml(md);
}

export function renderTo(md, elementId) {
    document.getElementById(elementId).innerHTML = createConverter().makeHtml(md);
}