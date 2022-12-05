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