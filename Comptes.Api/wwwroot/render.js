let guard = 0;

let renderers = [];
function addRenderer(predicate, func) {
    renderers.push({ predicate: predicate, func: func })
}

class Renderer {
    constructor() {
        this.afterRenders = [];
    }

    afterRender(f) {
        this.afterRenders.push(f);
    }

    callAfterRender() {
        for (let f of this.afterRenders) {
            console.log('call after render');
            f();
        }
    }
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
        return 'null';
    let found = null;
    for (let renderer of renderers) {
        if (renderer.predicate(x, h, r))
            found = renderer;
    }
    return found.func(x, h, r);
}

function renderArrayAsList(x, h, r) {
    let html = `<ul>`;
    for (let elt of x)
        html += "<li>" + render(elt, h + '.[]', r) + '</li>';
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


function renderObjectAsList(x, h, r) {
    let html = `<ul>`;
    let keys = Object.keys(x);
    for (let k of keys)
        html += `<li>${k} : ${render(x[k], h + '.' + k, r)}</li>`;
    html += "</ul>"
    return html;
}

function field(h) {
    let split = h.split('.');
    let f = split[split.length - 1];
    return f;
}

function renderForm(x, h, r) {
    r.afterRender(() => document.getElementById(h).addEventListener('submit', (evt) => {
        evt.preventDefault();

        let keys = Object.keys(x.Command);
        for (let k of keys) {
            x.Command[k] = document.getElementById(h + "." + k).value;
        }

        console.log('submit ' + h);
        formCall(x);
        return false;
    }));

    return `<form id="${h}">
                ${renderInputs(x, h, r)}
                <button type="submit" class="btn btn-primary">${field(h)}</button>
            </form>`;
}

function renderInputs(form, h) {
    let html = '';
    let keys = Object.keys(form.Command);
    for (let k of keys) {
        //console.log('    input : ' + k);
        html += `<div class="form-group">
                    <label for="${k}">${k}</label>
                    <input class="form-control" id="${h}.${k}" class="${h}.${k}"></input>
                </div>`;
    }
    return html;
}

function href(x) {
    return x['href'].replace('/api', '');
}

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
        r.afterRender(async () => {
            let response = await call(x);
            x.Result = response;
            renderTo(response, h, document.getElementById(h));
        });
        return `<div id="${h}"></div>`;
    });
addRenderer(
    (x, h, r) => x["IsForm"],
    (x, h, r) => {
        return renderForm(x, h, r)
    });
addRenderer(
    (x, h, r) => x["href"],
    (x, h, r) => `<a href="${href(x)}"> ${field(h)} </a><br/>`);
addRenderer(
    (x, h, r) => x.IsSvg,
    (x, h, r) => `${field(h)} :<br/> ${x.Svg}`);



