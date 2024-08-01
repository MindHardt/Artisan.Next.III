// noinspection SpellCheckingInspection
const createConverter = () => new showdown.Converter({
    emoji: true,
    parseImgDimensions: true,
    strikethrough: true,
    simplifiedAutoLink: true,
    tasklists: true,
});

export function renderMd(md) {
    const html = createConverter().makeHtml(md);
    const result = document.createElement('div');
    result.innerHTML = html;
    result.querySelectorAll('a').forEach(a => a.className = 'text-decoration-none text-link');
    return result.innerHTML;
}

export function renderTo(md, elementId) {
    document.getElementById(elementId).innerHTML = renderMd(md);
}