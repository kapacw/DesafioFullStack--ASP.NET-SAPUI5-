sap.ui.define([
    "tarefas/front/config/BaseUrl"
], function (BaseUrl) {
    "use strict";

    const baseUrl = BaseUrl.baseUrl;

    return {
        // POST /sync
        sincronizarBanco: async function () {
            const response = await fetch(`${baseUrl}/sync`, {
                method: "POST"
            });
            if (!response.ok) throw await response.json();
            return await response.json();
        }
    };
});
