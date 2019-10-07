


function renderPage(page, h, r) {

    let pageTypeName = page.PageTypeName;
    pageTypeName = pageTypeName.substring(pageTypeName.lastIndexOf('.') + 1);


    let htmlOnInits = page.OnInits.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');
    let htmlOnSubmits = page.OnSubmits.map(b => " - " + b.From.join('.') + ' =&gt; ' + b.To.join('.')).join('<br/>');


    if (htmlOnInits.length > 0)
        htmlOnInits = "OnInits :<br/>" + htmlOnInits + "<br/>";
    if (htmlOnSubmits.length > 0)
        htmlOnSubmits = "OnSubmits :<br/>" + htmlOnSubmits + "<br/>";

    let page2 = constructPageToRender(page);

    return `${renderMenu(page['Menu'], 'Menu', r)}
            <div class="container" style="margin-top: 90px">
                <b>${pageTypeName}</b>
                ${r.render(page2, pageTypeName)}
                <div style="font-size: 12px">${htmlOnInits + htmlOnSubmits}</div>
            </div>`;
}

function constructPageToRender(page) {
    console.log('fields to render :');
    let inputs = page.OnSubmits.map(x => x.From[0]);
    let page2 = {};
    for (let k of Object.keys(page)) {
        if (k == 'PageQuery' || k == 'OnSubmits' || k == 'OnInits' || k == 'PageTypeName')
            continue;
        if (k == 'Menu')
            continue;
        if (-1 != inputs.indexOf(k)) {
            continue;
        }
        console.log('  ' + k);
        page2[k] = page[k];
    }
    return page2;
}

function applyOnInits(page) {
    console.log('applyOnInits');
    let pageTypeName = page.PageTypeName;
    pageTypeName = pageTypeName.substring(pageTypeName.lastIndexOf('.') + 1);

    for (let init of page.OnInits) {
        if (valueExists(page, init.From)) {
            let v = getValue(page, init.From);

            console.log('applyOnInits setValue ' + init.To.join('.'), v);
            setValue(page, init.To, v);

            // update Inputs from models
            for (var input of _pageinputs) {
                input.updateFromModel();
            }
        }
    }
}


function applyOnSubmits(page) {
    console.log('applyOnSubmits');
    for (let s of page.OnSubmits) {
        console.log('  ' + s.From.join('.') + ' => ' + s.To.join('.'));
        let v = getValue(page, s.From);
        console.log('  ' + v);
        setValue(page, s.To, v);
    }
}

function applyInputsToModel(page) {
    for (var input of _pageinputs)
        input.updateModel();
}