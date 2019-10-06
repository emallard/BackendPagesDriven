function renderArrayAsList(x, h, r) {
    let html = `<ul>`;
    for (var i = 0; i < x.length; ++i)
        html += "<li>" + r.render(x[i], h + `[${i}]`) + '</li>';
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

    if (x == null || x.length == 0)
        return '[No elements in array]';

    if (options == null) {
        if (_renderers.findSpecific(x[0], h, r)) {
            return this.renderArrayAsList(x, h, r);
        }
    }

    if (options == null)
        options = {};
    if (options.columns == null) {
        options.columns = [];
        let keys = Object.keys(x[0]);
        for (let k of keys)
            options.columns.push({ "title": k, "data": k });
    }

    return `<table class="table table-responsive" id="${h}">
                <thead>
                    ${renderTableHeaders(options)}
                </thead>
                <tbody>
                    ${renderTableRows(x, h, r, options)}
                </tbody>
            </table>
            `
}

function renderTableHeaders(options) {
    let html = options.columns.map(x => '<th>' + x.title + '</th>').join('');
    return '<tr>' + html + '</tr>';
}

function renderTableRows(data, h, r, options) {
    return data.map((x, i) => renderTableRow(x, h + `[${i}]`, r, options)).join('');
}

function renderTableRow(data, h, r, options) {

    let htmls = options.columns.map(column => {
        return r.render(data[column.data], h + '.' + column.data)
    });
    return '<tr>' + htmls.map(html => `<td>${html}</td>`).join('') + '</tr>';
}

function renderObjectAsList(x, h, r) {
    let html = `<ul>`;
    let keys = Object.keys(x);
    for (let k of keys)
        html += `<li>${k} : ${r.render(x[k], h + '.' + k)}</li>`;
    html += "</ul>"
    return html;
}



function href(x) {
    return x['href'].replace('/api', '');
}


_renderers.push(
    (x, h, r) => true,
    (x, h, r) => '' + x);
_renderers.push(
    (x, h, r) => typeof (x) == 'object',
    (x, h, r) => renderObjectAsList(x, h, r));
_renderers.push(
    (x, h, r) => Array.isArray(x),
    (x, h, r) => renderArrayAsTable(x, h, r));
_renderers.push(
    (x, h, r) => typeof (x) == 'string',
    (x, h, r) => `${x}`);
_renderers.push(
    (x, h, r) => x["IsAsyncCall"] && x["OnInit"],
    (x, h, r) => {
        let hresult = h + '.Result';
        r.afterRender(async () => {
            let response = await call(x);
            x.Result = response;
            r.renderTo(response, hresult, document.getElementById(hresult));
        });
        return `<div id="${hresult}"></div>`;
        /*
        return `<ul>
                    <li>Result
                        <div id="${hresult}"></div>
                    </li>
                </ul>`;
        */
    });
_renderers.push(
    (x, h, r) => x["IsAsyncCall"] && !x["OnInit"],
    (x, h, r) => {
        r.afterRender(() => document.getElementById(h).addEventListener('click', () => call(x)))
        return `<button class="btn btn-success" id="${h}"> ${field(h)} </button>`;
    });
_renderers.push(
    (x, h, r) => x["IsForm"],
    (x, h, r) => renderForm2(x, h, r));
_renderers.push(
    (x, h, r) => x["href"],
    (x, h, r) => `<a id="${h}" href="${href(x)}"> ${field(h)} </a><br/>`);

_renderers.push(
    (x, h, r) => x.IsSvg,
    (x, h, r) => `${x.Svg}`.replace('<svg', `<svg id="${h + '.Svg'}"`));
_renderers.push(
    (x, h, r) => x.IsPageLink,
    (x, h, r) => `<a "id="${h}" href="${href(x.PageQuery)}"> ${x.Text} </a><br/>`);
_renderers.push(
    (x, h, r) => h == '',
    (x, h, r) => renderPage(x, h, r));