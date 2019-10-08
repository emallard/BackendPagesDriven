
_renderers.push(
    (x, id, r) => id == 'PageGraph.Result' && r.page.PageTypeName.endsWith('TestPageResponse'),
    (x, id, r) => {
        r.afterRender(() => setTimeout(() =>
            drawInGraph(
                _page.TestReport.Result.Path.Items,
                document.getElementById('PageGraph.Result.Svg'))
            , 1000));
        return `${x.Svg}`.replace('<svg', `<svg id="${id + '.Svg'}"`);
    }
);
