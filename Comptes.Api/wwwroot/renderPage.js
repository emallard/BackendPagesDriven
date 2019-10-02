
function renderPage(page, h, r) {

    let pageTypeName = page.PageTypeName;
    pageTypeName = pageTypeName.substring(pageTypeName.lastIndexOf('.') + 1);


    let htmlInits = page.Inits.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');
    let htmlBindings = page.Bindings.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');
    let htmlInputs = page.Inputs.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');

    if (htmlInits.length > 0)
        htmlInits = "Inits :<br/>" + htmlInits + "<br/>";
    if (htmlBindings.length > 0)
        htmlBindings = "Bindings :<br/>" + htmlBindings + "<br/>";
    if (htmlInputs.length > 0)
        htmlInputs = "Inputs :<br/>" + htmlInputs + "<br/>";

    let page2 = constructPageToRender(page);

    return `<b>${pageTypeName}</b>
            ${render(page2, pageTypeName, r)}
            <div style="font-size: 12px">${htmlInits + htmlBindings + htmlInputs}</div>`;
}

function constructPageToRender(page) {
    console.log('fields to render :');
    let inputs = page.Inputs.map(x => x.From.join('.'));
    let page2 = {};
    for (let k of Object.keys(page)) {
        if (k == 'PageQuery' || k == 'Bindings' || k == 'Inits' || k == 'Inputs' || k == 'PageTypeName')
            continue;
        if (-1 != inputs.indexOf(k)) {
            continue;
        }
        console.log('  ' + k);
        page2[k] = page[k];
    }
    return page2;
}

function applyInits() {
    let page = _page;
    console.log('applyInits');
    let pageTypeName = page.PageTypeName;
    pageTypeName = pageTypeName.substring(pageTypeName.lastIndexOf('.') + 1);

    for (let init of _page.Inits) {
        let v = getValue(_page, init.From);
        setFormInputValue(pageTypeName, init.To, v);
    }
}