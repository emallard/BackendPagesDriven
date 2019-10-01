
function drawInGraph(redirections, svgElt) {
    //console.log('drawInGraph', redirections, svgElt);
    let allTitles = svgElt.querySelectorAll('title');

    //console.log(allTitles);
    for (let r of redirections) {
        //console.log('pageTo : [' + r.PageTo.Text + ']');
        let title = _find(allTitles, x => x.innerHTML == r.PageTo.Text);
        let polygon = title.nextElementSibling;
        polygon.setAttribute('stroke', 'red');
        let text = polygon.nextElementSibling;
        text.setAttribute('fill', 'red');


        if (r.PageFrom != null) {
            let titleArrow = _find(allTitles, x => x.innerHTML == r.PageFrom.Text + '-&gt;' + r.PageTo.Text);
            let path = titleArrow.nextElementSibling.firstElementChild.firstElementChild;
            path.setAttribute('stroke', 'red');
            let polygon = path.nextElementSibling;
            //polygon.setAttribute('fill', 'red');
            polygon.setAttribute('stroke', 'red');

        }

    }
}

function _find(a, predicate) {
    for (let x of a)
        if (predicate(x))
            return x;
}