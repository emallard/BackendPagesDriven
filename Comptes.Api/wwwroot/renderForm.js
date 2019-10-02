function renderForm(x, h, r) {
    r.afterRender(() => document.getElementById(h).addEventListener('submit', (evt) => {
        evt.preventDefault();

        let keys = Object.keys(x.Command);
        for (let k of keys) {
            x.Command[k] = document.getElementById(h + ".Command." + k).value;
        }

        console.log('submit ' + h);
        formCall(x);
        return false;
    }));

    return `<form id="${h}">
                ${renderInputs(x, h + '.Command', r)}
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


function renderForm2(page, form, h, r) {

    r.afterRender(() => document.getElementById(h).addEventListener('submit', (evt) => {
        evt.preventDefault();

        let keys = Object.keys(form.Command);
        for (let k of keys) {
            form.Command[k] = document.getElementById(h + ".Command." + k).value;
        }

        console.log('submit ' + h);
        formCall(form);
        return false;
    }));


    var h2 = pathWithoutPage(h);
    var pageName = getPageName(h);

    var inputs = page.Inputs;
    var command = form.Command;

    var fieldPathsInForm = [];
    for (let k of Object.keys(command)) {
        fieldPathsInForm.push(h2 + '.Command.' + k);
    }

    // replace command paths with inputs paths
    for (let input of inputs) {
        var inputToReplace = input.To.join('.');
        var foundIndex = fieldPathsInForm.indexOf(inputToReplace);

        console.log('inputToReplace : ' + inputToReplace + ' : ' + foundIndex);
        if (foundIndex >= 0)
            fieldPathsInForm[foundIndex] = input.From.join('.');
    }

    console.log('renderForm2 : ' + h, fieldPathsInForm);

    let html = '';
    for (let f of fieldPathsInForm) {
        console.log('render input : ' + f);
        console.log(getValueFromPath(page, f));
        html += renderInput(getValueFromPath(page, f), pageName + '.' + f, r);
    }

    return `<form id="${h}">
                ${html}
                <button type="submit" class="btn btn-primary">${field(h)}</button>
            </form>`;
}

function renderInput(x, h, r) {
    var f = field(h);

    if (x._TypeName != null) {
        if (x._TypeName == 'string')
            return `<div class="form-group">
                        <label for="${h}">${f}</label>
                        <input class="form-control" id="${h}"></input>
                    </div>`;

        if (x._TypeName == 'double')
            return `<div class="form-group">
                        <label for="${h}">${f}</label>
                        <input class="form-control" type="number" id="${h}"></input>
                    </div>`;

        if (x._TypeName.startsWith('ID<'))
            return `<div class="form-group">
                        <label for="${h}">${f}</label>
                        <input class="form-control" id="${h}"></input>
                    </div>`;
    }
    if (x["IsSelect"])
        return `<div class="form-group">
            <label for="${h}">${f}</label>
            <select class="form-control" id="${h}">
                <option>kikoo</option>
            </select>
        </div>`;

    return `[INPUT ${x._TypeName}]`;

}