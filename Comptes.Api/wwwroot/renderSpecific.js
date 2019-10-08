
_renderers.push(
    (x, id, r) => id == 'PageGraph.Result' && r.page.PageTypeName.endsWith('TestPageResponse'),
    (x, id, r) => {
        r.afterRender(() => setTimeout(() =>
            drawTestInGraph(
                _page.TestReport.Result.Path.Items,
                document.getElementById('PageGraph.Result.Svg'))
            , 1000));
        return `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`);
    }
);

_renderers.push(
    (x, id, r) => id == 'PageGraph.Result' && r.page.PageTypeName.endsWith('PagePage'),
    (x, id, r) => {
        r.afterRender(() => setTimeout(() => {
            drawPageInGraph(
                _page.PageReport.Result.PageName,
                document.getElementById('PageGraph.Result.Svg'));

            addPageLinksToGraph(
                _page.Pages.Result,
                document.getElementById('PageGraph.Result.Svg'));
        }
            , 1000));
        return `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`);
    }
);