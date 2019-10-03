let guard = 0;

let renderers = [];
function addRenderer(predicate, func) {
    renderers.push({ predicate: predicate, func: func })
}

function renderTo(x, h, elt) {
    let renderer = new Renderer();
    elt.innerHTML = render(x, h, renderer);
    setTimeout(() => renderer.callAfterRender(), 0);
}

function render(x, h, r) {
    console.log("render " + h);
    guard++;
    if (guard > 1000) {
        console.log("GUARD TOO MANY CALLS : " + guard);
        return "";
    }

    if (x == null)
        return '';
    let found = null;
    for (let renderer of renderers) {
        if (renderer.predicate(x, h, r))
            found = renderer;
    }
    if (found == null)
        console.error('not found renderer : ' + h, x);
    return found.func(x, h, r);
}

function renderArrayAsList(x, h, r) {
    let html = `<ul>`;
    for (var i = 0; i < x.length; ++i)
        html += "<li>" + render(x[i], h + `.[${i}]`, r) + '</li>';
    html += "</ul>"
    return html;
}

function renderArrayAsDatatable(x, h, r, options) {
    r.afterRender(() => {
        console.log('renderArrayAsDatatable : ' + document.getElementById(h), options);
        $(document.getElementById(h)).dataTable(
            options
        )
    });
    return `<table id="${h}"></table>`
}

function renderArrayAsDatatable2(x, h, r, options) {
    r.afterRender(() => {
        $(document.getElementById(h)).dataTable(

        )
    });
    return renderArrayAsTable(x, h, r, options);
}

function renderArrayAsTable(x, h, r, options) {
    return `<table class="table" id="${h}">
                <thead>
                    ${renderTableHeaders(options)}
                </thead>
                <tbody>
                    ${renderTableRows(x, options, h, r)}
                </tbody>
            </table>
            `
}

function renderTableHeaders(options) {
    let html = options.columns.map(x => '<th>' + x.title + '</th>').join('');
    return '<tr>' + html + '</tr>';
}

function renderTableRows(data, options, h, r) {
    return data.map((x, i) => renderTableRow(x, options, h + `[${i}].`, r)).join('');
}

function renderTableRow(data, options, h, r) {
    let htmls = options.columns.map(column => render(data[column.data], h + column.data, r));
    return '<tr>' + htmls.map(html => `<td>${html}</td>`).join('') + '</tr>';
}

function renderObjectAsList(x, h, r) {
    let html = `<ul>`;
    let keys = Object.keys(x);
    for (let k of keys)
        html += `<li>${k} : ${render(x[k], h + '.' + k, r)}</li>`;
    html += "</ul>"
    return html;
}



function href(x) {
    return x['href'].replace('/api', '');
}


addRenderer(
    (x, h, r) => true,
    (x, h, r) => '' + x);
addRenderer(
    (x, h, r) => typeof (x) == 'object',
    (x, h, r) => renderObjectAsList(x, h, r));
addRenderer(
    (x, h, r) => Array.isArray(x),
    (x, h, r) => renderArrayAsList(x, h, r));
addRenderer(
    (x, h, r) => typeof (x) == 'string',
    (x, h, r) => `${x}`);
addRenderer(
    (x, h, r) => x["IsAsyncCall"],
    (x, h, r) => {
        let hresult = h + '.Result';
        r.afterRender(async () => {
            let response = await call(x);
            x.Result = response;
            renderTo(response, hresult, document.getElementById(hresult));
            applyOnInits();
        });
        return `<ul>
                    <li>Result
                        <div id="${hresult}"></div>
                    </li>
                </ul>`;
    });
addRenderer(
    (x, h, r) => x["IsForm"],
    (x, h, r) => renderForm2(_page, x, h, r));
addRenderer(
    (x, h, r) => x["href"],
    (x, h, r) => `<a id="${h}" href="${href(x)}"> ${field(h)} </a><br/>`);
addRenderer(
    (x, h, r) => x.IsSvg,
    (x, h, r) => `${x.Svg}`.replace('<svg', `<svg id="${h + '.Svg'}"`));

addRenderer(
    (x, h, r) => x.IsLinkModel,
    (x, h, r) => `<a "id="${h}" href="${href(x.PageQuery)}"> ${x.Text} </a><br/>`);
addRenderer(
    (x, h, r) => h == '',
    (x, h, r) => renderPage(x, h, r));