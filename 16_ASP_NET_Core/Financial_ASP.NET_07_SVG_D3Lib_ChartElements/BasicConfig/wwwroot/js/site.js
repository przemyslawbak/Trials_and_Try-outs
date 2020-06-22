function creatHouse(svg) {
    var house = svg.append("defs").append("g")
        .attr("id", "house")
        .style("fill", "red")
        .style("fill-opacity", 0.5)
        .style("stroke", "black")
        .style("stroke-width", "2");
    house.append("path")
        .attr("d", "M50 10 90 40 H10 Z")
    house.append("rect")
        .attr("x", 10)
        .attr("y", 40)
        .attr("width", 80)
        .attr("height", 50);
    house.append("rect")
        .attr("x", 60)
        .attr("y", 60)
        .attr("width", 20)
        .attr("height", 30)
        .style("fill", "none");
}

function createData() {
    var dat = [];
    for (var i = 0; i < 200; i++) {
        var ran = Math.random() * 100;
        var r = Math.round(ran / 3);
        var x = Math.round(400 * Math.random());
        var y = Math.round(400 * Math.random());
        dat.push({ "x": x, "y": y, "radius": r, "color": ran });
    }
    return dat;
}

function setAxesGridlines(svgSelection, options) {
    // define default values
    var xdomain = options.xdomain || [0, 100];
    var ydomain = options.ydomain || [0, 100];
    var width = options.width || 400;
    var height = options.height || 300;
    var margin = options.margin || { top: 20, right: 20, bottom: 20, left: 20 };
    var isTimeAxis = options.isTimeAxis || false;
    var isGridline = options.isGridline || true;
    var w = width - margin.left - margin.right;
    var h = height - margin.top - margin.bottom;
    var xscale, yscale;
    var svg = d3.select(svgSelection)
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
    // box for chart area
    svg.append("rect")
        .attr("width", w)
        .attr("height", h)
        .style("fill", "none")
        .style("stroke", "black")
        .style("stroke-width", 0.2);
    //X-axis:
    if (!isTimeAxis) {
        xscale = d3.scaleLinear().domain(xdomain).range([0, w]);
        var xaxis = d3.axisBottom().scale(xscale);
        svg.append("g")
            .attr("class", "x-axis")
            .attr("transform", "translate(0," + h + ")")
            .call(xaxis);
    } else {
        xscale = d3.scaleTime().domain(xdomain).range([0, w]);
        var xaxis1 = d3.axisBottom().scale(xscale);
        svg.append("g")
            .attr("class", "x-axis")
            .attr("transform", "translate(0," + h + ")")
            .call(xaxis1)
            .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-0.8em")
            .attr("dy", "0.15em")
            .attr("transform", "rotate(-60)");
    }
    //Y-axis:
    yscale = d3.scaleLinear().domain(ydomain).range([h, 0]);
    var yaxis = d3.axisLeft().scale(yscale);
    svg.append("g")
        .attr("class", "y-axis")
        .call(yaxis);
    //grid lines:
    if (isGridline) {
        svg.selectAll("line.ygrid")
            .data(yscale.ticks())
            .enter()
            .append("line")
            .attr("class", "ygrid")
            .attr("x1", 0)
            .attr("x2", w)
            .attr("y1", function (d) { return yscale(d); })
            .attr("y2", function (d) { return yscale(d); })
            .attr("stroke", "lightgray")
            .attr("stroke-width", "1px");
        svg.selectAll("line.xgrid")
            .data(xscale.ticks())
            .enter()
            .append("line")
            .attr("class", "xgrid")
            .attr("x1", function (d) { return xscale(d); })
            .attr("x2", function (d) { return xscale(d); })
            .attr("y1", 0)
            .attr("y2", h)
            .attr("stroke", "lightgray")
            .attr("stroke-width", "1px");
    }
    return { xscale, yscale, svg, w, h };
}