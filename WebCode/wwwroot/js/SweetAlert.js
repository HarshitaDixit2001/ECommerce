 
function showSuccessMessage(title, message) {
    Swal.fire({
        icon: 'success',
        title: title,
        html: `<div style="text-align: left; white-space: pre-wrap; font-size: 16px;"><strong>${message}</strong></div>`,
        confirmButtonText: 'Close',
        customClass: {
            htmlContainer: 'text-left'
        },
        width: '35%',
        allowOutsideClick: false,   // 🔒 Prevent closing by clicking outside
        allowEscapeKey: false,      // 🔒 Prevent closing by pressing ESC
        allowEnterKey: false        // 🔒 Prevent auto-confirm with Enter
    });
}


function showErrorLog(title, message) {
    Swal.fire({
        icon: 'error',
        title: title, 
        html: `<div style="text-align: left; white-space: pre-wrap; font-size: 16px;"><strong> ${message}</strong></div>`,
        confirmButtonText: 'Close',
        customClass: {
            htmlContainer: 'text-left'
        },
        width: '35%',
    });
}


function showWarning(title, message, onConfirm) {
    Swal.fire({
        icon: 'warning',
        title: title,
        text: `<span style="font-size: 16px;">${message}</span>`,
        showCancelButton: true,
        confirmButtonColor: '#008000',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
        if (result.isConfirmed) {
            onConfirm();
        }
    });
}


function showToastSuccess(message) {
    Swal.fire({
        toast: true,
        position: 'bottom-end',
        icon: "success",
        title: `<span style="font-size: 16px;">${message}</span>`,
        showConfirmButton: false,
        timer: 700,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });
}

function showToastError(message) {
    Swal.fire({
        toast: true,
        position: 'bottom-end',
        icon: "error",
        title: `<span style="font-size: 16px;">${message}</span>`,
        showConfirmButton: false,
        timer: 1000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });
}



function ConfirmationMessage(title, message, confirmCallback) {
    Swal.fire({
        icon: 'question',
        title: title,
        html: `<div style="text-align: left; white-space: pre-wrap; font-size: 16px;"><strong>${message}</strong></div>`,
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        customClass: { htmlContainer: 'text-left' },
        width: '35%'
    }).then((result) => {
        if (result.isConfirmed) {  confirmCallback(); }
    });
}