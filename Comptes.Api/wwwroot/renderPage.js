class Page {
    constructor() {
        _page = null;
    }
    renderTo() { }

    applyOnInits() { }

    applyOnSubmits() { }
}

_renderConf = new RenderConf();
class RenderConf {
    renderers = [];
}

class RenderContext {
    page = null;
    afterRender(callback) { }
    renderTo(x, h) { }
}


function renderPage(page, h, r) {

    let pageTypeName = page.PageTypeName;
    pageTypeName = pageTypeName.substring(pageTypeName.lastIndexOf('.') + 1);


    let htmlOnInits = page.OnInits.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');
    let htmlOnSubmits = page.OnSubmits.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');
    //let htmlInputs = page.Inputs.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');

    if (htmlOnInits.length > 0)
        htmlOnInits = "OnInits :<br/>" + htmlOnInits + "<br/>";
    if (htmlOnSubmits.length > 0)
        htmlOnSubmits = "OnSubmits :<br/>" + htmlOnSubmits + "<br/>";
    //if (htmlInputs.length > 0)
    //    htmlInputs = "Inputs :<br/>" + htmlInputs + "<br/>";

    let page2 = constructPageToRender(page);

    return `<b>${pageTypeName}</b>
            ${render(page2, pageTypeName, r)}
            <div style="font-size: 12px">${htmlOnInits + htmlOnSubmits}</div>`;
}

function constructPageToRender(page) {
    console.log('fields to render :');
    let inputs = page.Inputs.map(x => x.From.join('.'));
    let page2 = {};
    for (let k of Object.keys(page)) {
        if (k == 'PageQuery' || k == 'OnSubmits' || k == 'OnInits' || k == 'Inputs' || k == 'PageTypeName')
            continue;
        if (-1 != inputs.indexOf(k)) {
            continue;
        }
        console.log('  ' + k);
        page2[k] = page[k];
    }
    return page2;
}

function applyOnInits() {
    let page = _page;
    console.log('applyOnInits');
    let pageTypeName = page.PageTypeName;
    pageTypeName = pageTypeName.substring(pageTypeName.lastIndexOf('.') + 1);

    for (let init of _page.OnInits) {
        let v = getValue(_page, init.From);
        setFormInputValue(pageTypeName, init.To, v);
    }
}

function applyOnSubmits(page) {
    console.log('applyOnSubmits');
    for (let s of page.OnSubmits) {
        console.log('  ' + s.From.To.Join('.') + ' => ' + s.ToJoin('.'));
        let v = getValue(page, s.From);
        console.log('  ' + v);
        setValue(page, s.To, v);
    }
}