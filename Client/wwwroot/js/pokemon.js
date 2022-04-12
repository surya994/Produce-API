$(document).ready(function () {
    $('#tableSW').DataTable({
        "processing": true,
        "ajax": {
            "url": "https://pokeapi.co/api/v2/pokemon",
            "dataSrc": "results"
        },
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "name" },
            {
                "data" : "url",
                "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#detilModal" onclick="showDetil('${data}')">Detil</button></td>`
                }
            },
        ]
    });
});
function showDetil(url) {
    $.ajax({
        url: url,
        success: function (result) {
            console.log(result);

            var detilPoke = "";
            detilPoke += `<img src="${result.sprites.other.dream_world.front_default}" class="mx-auto d-block" style="height:150px" alt="Cinque Terre">`;
            detilPoke += `<h5 class="text-center my-3"><b>${result.name.toUpperCase()}</b></h5><p class="text-center">`
            $.each(result.types, function (key, val) {
                detilPoke += `<span class="badge badge-pill badge-primary"> ${val.type.name} </span> `
            })
            detilPoke += `</p>`
            $("#detilPoke").html(detilPoke);

            var infoPoke = "";
            infoPoke += `<tr><th width=30%>Species</th><td>${result.species.name}</td></tr>`;
            infoPoke += `<tr><th width=30%>Height</th><td>${result.height} inch</td></tr>`;
            infoPoke += `<tr><th width=30%>Weight</th><td>${result.weight} lbs</td></tr>`;
            infoPoke += `<tr><th width=30%>Base Exp</th><td>${result.base_experience}</td></tr>`;
            $("#infoPoke").html(infoPoke);

            var skillPoke = "";
            $.each(result.abilities, function (key, val) {
                skillPoke += `<tr><td>${val.ability.name}</td><td>${effectPoke(val.ability.url)}</tr>`
            })
            $("#skillPoke").html(skillPoke);

            var statusPoke = "";
            statusPoke += `<tr><td>${result.stats[0].stat.name}</td><td>${result.stats[0].effort}</td><td><div class="progress"><div class="progress-bar bg-success" style="width:${result.stats[0].base_stat}%">${result.stats[0].base_stat}</div></div></td></tr>`
            statusPoke += `<tr><td>${result.stats[1].stat.name}</td><td>${result.stats[1].effort}</td><td><div class="progress"><div class="progress-bar bg-danger" style="width:${result.stats[1].base_stat}%">${result.stats[1].base_stat}</div></div></td></tr>`
            statusPoke += `<tr><td>${result.stats[2].stat.name}</td><td>${result.stats[2].effort}</td><td><div class="progress"><div class="progress-bar bg-info" style="width:${result.stats[2].base_stat}%">${result.stats[2].base_stat}</div></div></td></tr>`
            statusPoke += `<tr><td>${result.stats[3].stat.name}</td><td>${result.stats[3].effort}</td><td><div class="progress"><div class="progress-bar progress-bar-striped bg-danger" style="width:${result.stats[3].base_stat}%">${result.stats[3].base_stat}</div></div></td></tr>`
            statusPoke += `<tr><td>${result.stats[4].stat.name}</td><td>${result.stats[4].effort}</td><td><div class="progress"><div class="progress-bar progress-bar-striped bg-info" style="width:${result.stats[4].base_stat}%">${result.stats[4].base_stat}</div></div></td></tr>`
            statusPoke += `<tr><td>${result.stats[5].stat.name}</td><td>${result.stats[5].effort}</td><td><div class="progress"><div class="progress-bar progress-bar-striped progress-bar-animated bg-warning" style="width:${result.stats[5].base_stat}%">${result.stats[5].base_stat}</div></div></td></tr>`
            $("#statusPoke").html(statusPoke);
        }
    })
}
function effectPoke(url) {
    var skill;
    $.ajax({
        async: false,
        url: url,
        dataType: 'json',
        success: function (data) {
            $.each(data.effect_entries, function (key, val) {
                if (val.language.name === "en") {
                    skill = val.short_effect;
                }
            })
        }
    });
    return skill;
}