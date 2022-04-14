$(document).ready(function () {
    selectUniversity();
    tableEmployee;
});
/* Dropdown University */
function selectUniversity() {
    $.ajax({
        url: '/Universities/GetAll/',
        success: function (data) {
            $.each(data, function (key, val) {
                $('#university').append($('<option>', { value: val.id, text: val.name }));
                $('#universityUpdate').append($('<option>', { value: val.id, text: val.name }));
            })
        }
    });
}
/* Table Employee */
var tableEmployee = $('#tableEmployee').DataTable({
    "processing": true,
    "ajax": {
        "url": "/employees/GetMasterAll/",
        "dataSrc": ""
    },
    "dom": "<'row'<'col-md-4'l><'col-md-4' B><'col-md-4'f>>" + "<'row'<'col-md-12'tr>>" + "<'row'<'col-5'i><'col-7'p>>",
    "buttons": [
        {
            extend: 'print',
            text: '<i class="fas fa-print"></i>',
            exportOptions: {
                columns: 'th:not(:last-child):visible'
            }
        },
        {
            extend: 'copyHtml5',
            text: '<i class="far fa-copy"></i>',
            exportOptions: {
                columns: 'th:not(:last-child):visible'
            }
        },
        {
            extend: 'pdfHtml5',
            text: '<i class="far fa-file-pdf"></i>',
            orientation: 'landscape',
            exportOptions: {
                columns: 'th:not(:last-child):visible'
            }
        },
        {
            extend: 'excelHtml5',
            text: '<i class="far fa-file-excel"></i>',
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
        {
            "data": null,
            "render": function (data, type, row) {
                return `${row["firstName"]} ${row["lastName"]}`;
            }
        },
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
                return `<button type="button" class="btn btn-sm btn-danger" onClick="Delete('${data}')" data-toggle="tooltip" title="Hapus Data"><i class="fas fa-fw fa-trash-alt"></i></button></td>
                            <button type="button" class="btn btn-sm btn-warning" onClick="Edit('${data}')" data-toggle="tooltip" title="Edit Data"><i class="fas fa-fw fa-edit"></i></button></td>`
            }
        },
    ]
});
/* Add Employee */
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
        url: "/accounts/register/",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(obj),
        beforeSend: function () {
            swal.fire({
                title: 'Menunggu',
                html: 'Memproses data',
                didOpen: () => {
                    swal.showLoading()
                }
            })
        }
    }).done((result) => {
        tableEmployee.ajax.reload();
        $(".needs-validation").removeClass('was-validated');
        $('#formInput').find('form').trigger('reset');
        $('#formInput').modal('hide');
        console.log(result)
        Swal.fire(
            'Berhasil',
            result,
            'success'
        )
    }).fail((error) => {
        console.log(error)
        Swal.fire(
            'Gagal',
            error.responseText,
            'error'
        )
    })
}
/* Delete Employee */
function Delete(nik){
    event.preventDefault();
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
                url: "/employees/delete/" + nik,
                type: "DELETE",
                beforeSend: function () {
                    swal.fire({
                        title: 'Menunggu',
                        html: 'Memproses data',
                        didOpen: () => {
                            swal.showLoading()
                        }
                    })
                }
            }).done((result) => {
                tableEmployee.ajax.reload();
                Swal.fire(
                    'Berhasil',
                    result,
                    'success'
                )
            }).fail((error) => {
                Swal.fire(
                    'Gagal',
                    error.responseText,
                    'error'
                )
            })
        }
    })
}
/* Edit Employee */
function Edit(nik) {
    $('#formUpdate').modal('show');
    $.ajax({
        url: '/employees/GetMaster/' + nik,
        success: function (data) {
            console.log(data)
            $("#nikUpdate").val(data[0].nik);
            $("#educationIdUpdate").val(data[0].educationId);
            $("#firstNameUpdate").val(data[0].firstName);
            $("#lastNameUpdate").val(data[0].lastName);
            $("#birthDateUpdate").val($.datepicker.formatDate('yy-mm-dd', new Date(data[0].birthDate)));
            $("#genderUpdate").find('option:contains("' + data[0].gender +'")').prop('selected', true);
            $("#phoneUpdate").val(data[0].phone);
            $("#salaryUpdate").val(data[0].salary);
            $("#universityUpdate").find('option:contains("' + data[0].universityName + '")').prop('selected', true);
            $("#degreeUpdate").find('option:contains("' + data[0].degree + '")').prop('selected', true);
            $("#gpaUpdate").val(data[0].gpa);
            $("#emailUpdate").val(data[0].email);
        }
    });
}
$("#submitButtonUpdate").click(function () {
    var form = $(".needs-validation-update")
    event.preventDefault();
    if (form[0].checkValidity() === false) {
        event.stopPropagation();
    } else {
        Update();
    }
    form.addClass('was-validated');
})
function Update() {
    var obj = new Object();
    obj.NIK = $("#nikUpdate").val();
    obj.FirstName = $("#firstNameUpdate").val();
    obj.LastName = $("#lastNameUpdate").val();
    obj.BirthDate = $("#birthDateUpdate").val();
    obj.Gender = $("#genderUpdate").val();
    obj.Phone = $("#phoneUpdate").val();
    obj.Salary = $("#salaryUpdate").val();
    obj.UniversityId = $("#universityUpdate").val();
    obj.EducationId = $("#educationIdUpdate").val();
    obj.Degree = $("#degreeUpdate").val();
    obj.GPA = $("#gpaUpdate").val();
    obj.Email = $("#emailUpdate").val();
    console.log(obj);
    $.ajax({
        url: "/accounts/update/",
        type: "PUT",
        contentType: 'application/json',
        data: JSON.stringify(obj),
        beforeSend: function () {
            swal.fire({
                title: 'Menunggu',
                html: 'Memproses data',
                didOpen: () => {
                    swal.showLoading()
                }
            })
        }
    }).done((result) => {
        tableEmployee.ajax.reload();
        $(".needs-validation-update").removeClass('was-validated');
        $('#formUpdate').modal('hide');
        Swal.fire(
            'Berhasil',
            result,
            'success'
        )
    }).fail((error) => {
        Swal.fire(
            'Gagal',
            error.responseText,
            'error'
        )
    })
}