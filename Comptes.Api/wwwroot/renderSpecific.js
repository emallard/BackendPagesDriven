_renderers.push(
    (x, id, r) => x['IsCsFile'],
    (x, id, r) => {
        r.afterRender(() => {
            //hljs.highlightBlock(document.getElementById(id));
            Prism.highlightAll();
        })
        let txt = x.Text.replace(/>/g, "&gt;").replace(/</g, "&lt;");
        return `<pre id="${id}"><code class="language-java">${txt}</code></pre>`
    }
);


_renderers.push(
    (x, id, r) => id == 'PageGraph.Result' && r.page.PageTypeName.endsWith('TestPageResponse'),
    (x, id, r) => {
        r.afterRender(() => {
            let svgElt = document.getElementById('PageGraph.Result.Svg');
            svgElt.removeAttribute("width");
            svgElt.removeAttribute("height");
        });

        r.afterRender(() => setTimeout(() => {
            let svgElt = document.getElementById('PageGraph.Result.Svg');
            drawTestInGraph(
                _page.TestReport.Result.Path.Items,
                svgElt
            )
                ;
            svgPanZoom(
                svgElt,
                { contain: true }
            );
        }
            , 1000));
        let svg = `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`);
        return `<div style="width:100%;overflow-x:hidden">${svg}</div>`;
    }
);

_renderers.push(
    (x, id, r) => id == 'PageGraph.Result' && r.page.PageTypeName.endsWith('PagePage'),
    (x, id, r) => {
        r.afterRender(() => {
            let svgElt = document.getElementById('PageGraph.Result.Svg');
            svgElt.removeAttribute("width");
            svgElt.removeAttribute("height");
        });

        r.afterRender(() => setTimeout(() => {
            let svgElt = document.getElementById('PageGraph.Result.Svg');

            drawPageInGraph(
                _page.PageReport.Result.PageName,
                svgElt);

            addPageLinksToGraph(
                _page.Pages.Result,
                svgElt);

            svgPanZoom(svgElt);
        }
            , 1000));
        let svg = `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`);
        return `<div style="width:100%;overflow-x:hidden">${svg}</div>`;

    }
);