
function drawTestInGraph(redirections, svgElt) {
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

function drawPageInGraph(pageName, svgElt) {
    console.log('drawPageInGraph ' + pageName);
    let allTitles = svgElt.querySelectorAll('title');

    let title = _find(allTitles, x => x.innerHTML == pageName);
    let polygon = title.nextElementSibling;
    polygon.setAttribute('stroke', 'red');
    let text = polygon.nextElementSibling;
    text.setAttribute('fill', 'red');
}


function addPageLinksToGraph(pageLinks, svgElt) {
    svgElt.setAttribute('xmlns:xlink', 'http://www.w3.org/1999/xlink');
    console.log('addPageLinksToGraph');
    let allTitles = svgElt.querySelectorAll('title');
    for (let pageLink of pageLinks) {
        let title = _find(allTitles, x => x.innerHTML == pageLink.Text);
        let g = title.parentElement;
        let gParent = g.parentElement;

        var a = document.createElement('a');
        a.title = pageLink.Text;
        a.setAttribute('xlink:href', href(pageLink.PageQuery));

        gParent.removeChild(g);
        a.appendChild(g);
        gParent.appendChild(a);
    }

    svgElt.innerHTML += '';
}

function _find(a, predicate) {
    for (let x of a)
        if (predicate(x))
            return x;
}