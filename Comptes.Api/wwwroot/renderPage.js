


function renderPage(page, id, r) {

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
                <ul>
                    ${renderObjectAsList(page2, '', r)}
                    
                </ul>
                <div style="font-size: 12px">${htmlOnInits + htmlOnSubmits}</div>
            </div>`;
}
//${Object.keys(page2).map(k => `<li>${k} : ${r.render(page2[k], k)}</li>`).join('')}

function constructPageToRender(page) {
    console.log('fields to render :');
    let inputs = page.OnSubmits.map(x => x.From[0]);
    let page2 = {};
    for (let k of Object.keys(page)) {
        if (k == 'PageQuery' || k == 'OnSubmits' || k == 'OnInits' || k == 'PageTypeName' || k == 'RenderInfos')
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
