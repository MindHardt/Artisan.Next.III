function getTheme() {
    let theme = localStorage.getItem('theme');
    theme ??= window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    return theme;
}
function fixThemeIcon() {
    const themeIcon = document.getElementById('themeIcon');
    if (themeIcon === null) {
        return;
    }
    const theme = getTheme();
    const icon = theme === 'light' ? 'bi-sun-fill' : 'bi-moon-fill';
    themeIcon.className = 'text-warning bi ' + icon;
}
function setTheme(theme) {
    document.body.getAttributeNode('data-bs-theme').value = theme;
    localStorage.setItem('theme', theme);
    fixThemeIcon();
    console.log('Theme set to', theme)
}
function changeTheme() {
    const currentTheme = getTheme();
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    setTheme(newTheme);
}
function restoreTheme() {
    const theme = getTheme();
    const themeAttr = document.body.getAttributeNode('data-bs-theme') ?? document.createAttribute('data-bs-theme');
    document.body.attributes.setNamedItem(themeAttr);
    setTheme(theme);
}
restoreTheme();