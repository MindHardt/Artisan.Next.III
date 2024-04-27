function showModal(modalSelector) {
    const modalElement = document.querySelector(modalSelector);
    const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
    modal.show();
    const backdrop = document.getElementsByClassName('modal-backdrop show')[0];
    backdrop.parentNode.insertBefore(modalElement, backdrop);
}