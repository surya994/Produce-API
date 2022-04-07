// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*const animals = [
    { name: "garfield", species: "cat", class: { name: "mamalia" } },
    { name: "nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "tom", species: "cat", class: { name: "mamalia" } },
    { name: "garry", species: "cat", class: { name: "mamalia" } },
    { name: "dory", species: "fish", class: { name: "invertebrata" } },
    { name: "oren", species: "cat", class: { name: "mamalia" } },
    { name: "meong", species: "cat", class: { name: "mamalia" } },
    { name: "lele", species: "fish", class: { name: "invertebrata" } }

]
const onlyCat = [];
for (var i = 0; i < animals.length; i++) {
    if (animals[i].species=="cat") {
        onlyCat.push(animals[i])
    }
}
console.log(onlyCat);

for (var i = 0; i < animals.length; i++) {
    if (animals[i].species == "fish") {
        animals[i].class.name = "non mamalia";
    }
}
console.log(animals);*/


/* Pokemon */

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


/* Employee */
function selectUniversity() {
    $.ajax({
        url: 'https://localhost:44375/api/University/',
        success: function (data) {
            $.each(data.result, function (key, val) {
                $('#university').append($('<option>', { value: val.id, text: val.name }));
            })
        }
    });
}
$(document).ready(function () {
    selectUniversity();
    var tableEmployee = $('#tableEmployee').DataTable({
        "processing": true,
        "ajax": {
            "url": "https://localhost:44375/api/employee/master",
            "dataSrc": "result"
        },
        "dom": "<'row'<'col-md-3'l><'col-md-6'B><'col-md-3'f>>" + "<'row'<'col-md-12'tr>>" + "<'row'<'col-5'i><'col-7'p>>",
        "buttons": [
            {
                extend: 'print',
                exportOptions: {
                    columns: 'th:not(:last-child):visible'
                }
            },
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: 'th:not(:last-child):visible'
                }
            },
            {
                extend: 'pdfHtml5',
                orientation: 'landscape',
                exportOptions: {
                    columns: 'th:not(:last-child):visible'
                }
            },
            {
                extend: 'excelHtml5',
                exportOptions: {
                    columns: 'th:not(:last-child):visible'
                }
            },
            {
                extend: 'csvHtml5',
                exportOptions: {
                    columns: 'th:not(:last-child):visible'
                }
            },
            'colvis'
        ],
        "columnDefs": [
            {
                "orderable": false,
                "targets": 11
            }
        ],
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "nik" },
            { "data": "fullName" },
            { "data": "gender" },
            {
                "data": "birthDate",
                "render": function (data, type, row) {
                    return $.datepicker.formatDate('dd MM yy', new Date(data));
                }
            },
            {
                "data": "phone",
                "render": function (data, type, row) {
                    return data.replace(/^0+/, '+62')
                }
            },
            { "data": "email" },
            {
                "data": "salary",
                "render": function (data, type, row) {
                    return "Rp." + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
                }
            },
            { "data": "degree" },
            { "data": "gpa" },
            { "data": "universityName" },
            {
                "data": "nik",
                "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-danger delete-employee" data-id="${data}">Hapus</button></td>`
                }
            },
        ]
    });
    $("#submitButton").click(function () {
        var form = $(".needs-validation")
        event.preventDefault();
        if (form[0].checkValidity() === false) {
            event.stopPropagation();
        } else {
            Insert();
        }
        form.addClass('was-validated');
    })
    function Insert() {
        var obj = new Object();
        obj.FirstName = $("#firstName").val();
        obj.LastName = $("#lastName").val();
        obj.BirthDate = $("#birthDate").val();
        obj.Gender = $("#gender").val();
        obj.Phone = $("#phone").val();
        obj.Salary = $("#salary").val();
        obj.UniversityId = $("#university").val();
        obj.Degree = $("#degree").val();
        obj.GPA = $("#gpa").val();
        obj.Email = $("#email").val();
        obj.Password = $("#password").val();
        $.ajax({
            url: "https://localhost:44375/api/account/register",
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify(obj),
        }).done((result) => {
            tableEmployee.ajax.reload();
            $('#formInput').find('form').trigger('reset');
            $('#formInput').modal('hide');
            Swal.fire(
                'Berhasil',
                result.message,
                'success'
            )
        }).fail((error) => {
            Swal.fire(
                'Gagal',
                error.responseJSON.message,
                'error'
            )
        })
    }
    $("#tableEmployee").on('click', '.delete-employee', (function () {
        event.preventDefault();
        var id = $(this).data("id");
        Swal.fire({
            title: 'Hapus Data',
            text: "Anda yakin ingin menghapus data ini ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Ya',
            cancelButtonText: 'Batal'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "https://localhost:44375/api/employee/" + id,
                    type: "DELETE"
                }).done((result) => {
                    tableEmployee.ajax.reload();
                    Swal.fire(
                        'Berhasil',
                        result.message,
                        'success'
                    )
                }).fail((error) => {
                    Swal.fire(
                        'Gagal',
                        error.responseJSON.message,
                        'error'
                    )
                })
            }
        })
    }))
});
