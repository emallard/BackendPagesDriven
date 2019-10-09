function renderArrayAsList(x, id, r) {
    let html = `<ul>`;
    for (var i = 0; i < x.length; ++i)
        html += "<li>" + r.render(x[i], id + `[${i}]`) + '</li>';
    html += "</ul>"
    return html;
}

function renderArrayAsDatatable(x, id, r, options) {
    r.afterRender(() => {
        console.log('renderArrayAsDatatable : ' + document.getElementById(id), options);
        $(document.getElementById(id)).dataTable(
            options
        )
    });
    return `<table id="${id}"></table>`
}

function renderArrayAsDatatable2(x, id, r, options) {
    r.afterRender(() => {
        $(document.getElementById(id)).dataTable(

        )
    });
    return renderArrayAsTable(x, id, r, options);
}

function renderArrayAsTable(x, id, r, options) {

    if (x == null || x.length == 0)
        return '[No elements in array]';

    if (options == null) {
        if (_renderers.findSpecific(x[0], id, r)) {
            return this.renderArrayAsList(x, id, r);
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

    return `<table class="table table-responsive" id="${id}">
                <thead>
                    ${renderTableHeaders(options)}
                </thead>
                <tbody>
                    ${renderTableRows(x, id, r, options)}
                </tbody>
            </table>
            `
}

function renderTableHeaders(options) {
    let html = options.columns.map(x => '<th>' + x.title + '</th>').join('');
    return '<tr>' + html + '</tr>';
}

function renderTableRows(data, id, r, options) {
    return data.map((x, i) => renderTableRow(x, id + `[${i}]`, r, options)).join('');
}

function renderTableRow(data, id, r, options) {

    let htmls = options.columns.map(column => {
        return r.render(data[column.data], id + '.' + column.data)
    });
    return '<tr>' + htmls.map(html => `<td>${html}</td>`).join('') + '</tr>';
}

function renderObjectAsList(x, id, r) {
    let html = `<ul>`;
    let keys = Object.keys(x);
    for (let k of keys) {
        let childId = id == '' ? k : id + '.' + k;
        let renderInfo = _find(r.page.RenderInfos, x => x.Expression == childId);
        let hidden = false;
        if (renderInfo != null && renderInfo.Info == 'hidden') {
            hidden = true;
            console.log('  ' + childId + ' is hidden')
        }

        html += `<li ${hidden ? 'style="display:none"' : ''} >
                    ${k} : ${r.render(x[k], childId)}
                 </li>`;
    }

    html += "</ul>"
    return html;
}



function href(x) {
    return x['href'].replace('/api', '');
}


_renderers.push(
    (x, id, r) => true,
    (x, id, r) => '' + x);
_renderers.push(
    (x, id, r) => typeof (x) == 'object',
    (x, id, r) => renderObjectAsList(x, id, r));
_renderers.push(
    (x, id, r) => Array.isArray(x),
    (x, id, r) => renderArrayAsTable(x, id, r));
_renderers.push(
    (x, id, r) => typeof (x) == 'string',
    (x, id, r) => `${x}`);
_renderers.push(
    (x, id, r) => x["IsAsyncCall"] && x["OnInit"],
    (x, id, r) => {
        let hresult = id + '.Result';
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
    (x, id, r) => x["IsAsyncCall"] && !x["OnInit"],
    (x, id, r) => {
        r.afterRender(() => document.getElementById(id).addEventListener('click', () => call(x)))
        return `<button class="btn btn-success" id="${id}"> ${field(id)} </button>`;
    });
_renderers.push(
    (x, id, r) => x["IsForm"],
    (x, id, r) => renderForm2(x, id, r));
_renderers.push(
    (x, id, r) => x["href"],
    (x, id, r) => `<a id="${id}" href="${href(x)}"> ${field(id)} </a><br/>`);

_renderers.push(
    (x, id, r) => x.IsSvg,
    (x, id, r) => `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`));
_renderers.push(
    (x, id, r) => x.IsPageLink,
    (x, id, r) => `<a "id="${id}" href="${href(x.PageQuery)}"> ${x.Text} </a><br/>`);
_renderers.push(
    (x, id, r) => id == '',
    (x, id, r) => renderPage(x, id, r));