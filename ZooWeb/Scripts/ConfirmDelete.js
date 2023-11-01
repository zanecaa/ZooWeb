// https://groovetechnology.com/blog/how-to-use-javascript-in-asp-net-web-forms/
function confirmDelete(uniqueId, isDeleteClicked) {
	var deleteAnchor = 'deleteAnchor_' + uniqueId;
	var confirmDeleteAnchor = 'confirmDeleteAnchor_' + uniqueId;

	if (isDeleteClicked) {
		$('#' + deleteAnchor).hide();
		$('#' + confirmDeleteAnchor).show();
	} else {
		$('#' + deleteAnchor).show();
		$('#' + confirmDeleteAnchor).hide();
	}
}