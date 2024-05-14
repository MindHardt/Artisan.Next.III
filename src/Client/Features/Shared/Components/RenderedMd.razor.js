export function renderMd(md) {
    const converter = new showdown.Converter({
        emoji: true,
        parseImgDimensions: true,
        strikethrough: true,
        simplifiedAutoLink: true
    });

    return converter.makeHtml(md)
}