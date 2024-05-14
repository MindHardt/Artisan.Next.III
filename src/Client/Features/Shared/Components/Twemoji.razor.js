export function updateTwemoji(elementId) {
    const element = document.getElementById(elementId);
    twemoji.parse(element, {
        folder: 'svg',
        ext: '.svg',
        className: 'bi emoji'
    })
}