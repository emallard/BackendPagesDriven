
function field(id) {
    let split = id.split('.');
    let f = split[split.length - 1];
    return f;
}

function pathWithoutPage(id) {
    let split = id.split('.');
    return split.slice(1).join('.');
}

function getPageName(id) {
    let split = id.split('.');
    return split[0];
}

function valueExists(x, members) {
    let y = x;
    for (let i = 0; i < members.length; ++i) {
        y = y[members[i]];
        if (y == null)
            return false;
    }
    return y != null;
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
    y[members[members.length - 1]] = value;
}