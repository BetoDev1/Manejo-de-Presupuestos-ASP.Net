function inicializarFormularioTransacciones(urlObtenerCategorias) {
    $("#TipoOperacionId").change(async function () {
        const valorSeleccionado = $(this).val();

        const respuesta = await fetch(urlObtenerCategorias, {
            method: 'POST',
            body: valorSeleccionado,
            headers: {
                'Content-Type': 'application/json'
            }
        })

        const json = await respuesta.json();
        console.log(json);

        //Genero un arreglo de opciones
        const opciones = json.map(categoria => `<option value=${categoria.value}>${categoria.text}</option>`);

        //Inserto ese arreglo de opciones en el HTML
        $("#CategoriaId").html(opciones);
    })
}