function renderMenu(x, id, r) {
    let links = '';
    if (x != undefined)
        links = Object.keys(x).filter(k => k != 'IsMenu').map(k =>
            `<li class="nav-item">
                <a class="nav-link" href="${href(x[k])}">${k}</a>
            </li>`
        ).join('');


    return ` <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top py-3" id="${id}">
    <div class="container">
        <a class="navbar-brand js-scroll-trigger" href="/">Mes Comptes</a>
        <button class="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarResponsive">
            <ul class="navbar-nav ml-auto my-2 my-lg-0">
                ${links}
            </ul>
        </div>
    </div>
</nav>`;
}