export function downloadSvg(svgSelector, width, height, fileName) {
    const svg = document.querySelector(svgSelector);
    const doc = new PDFDocument({
        size: [width, height], // A5
        margins : {
            top: 10,
            bottom:10,
            left: 10,
            right: 10
        }
    });
    SVGtoPDF(doc.font('Helvetica'), svg, 0, 0, {
        assumePt: true,
        preserveAspectRatio: 'xMaxYMax',
        width: width
    });
    let stream = doc.pipe(blobStream());
    stream.on('finish', () => {
        let blob = stream.toBlob('application/pdf');
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        link.click();
    });
    doc.end();
}