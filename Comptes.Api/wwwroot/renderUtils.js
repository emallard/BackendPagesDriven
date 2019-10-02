
function field(h) {
    let split = h.split('.');
    let f = split[split.length - 1];
    return f;
}

function pathWithoutPage(h) {
    let split = h.split('.');
    return split.slice(1).join('.');
}

function getPageName(h) {
    let split = h.split('.');
    return split[0];
}

function getValueFromPath(x, path) {
    //console.log(' get ' + members.join('.'));
    var fields = path.split('.');
    let y = x;
    for (let i = 0; i < fields.length; ++i)
        y = y[fields[i]];
    return y;
}


function getValue(x, members) {
    //console.log(' get ' + members.join('.'));
    let y = x;
    for (let i = 0; i < members.length; ++i)
        y = y[members[i]];
    return y;
}

function setValue(x, members, value) {
    //console.log(' set ' + members.join('.') + ' = ' + value);
    let y = x;
    for (let i = 0; i < members.length - 1; ++i)
        y = y[members[i]];
    y[members.length - 1] = value;
}

function setFormInputValue(pageTypeName, members, value) {
    let id = pageTypeName + '.' + members.join('.');
    console.log(' setInput : ' + id + ' = ' + value);
    if (value['Id'] != null)
        value = value['Id'];
    document.getElementById(id).value = value;
}