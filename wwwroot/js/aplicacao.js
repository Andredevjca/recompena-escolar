
(() => {
    'use strict';

    const botaoMenu = document.getElementById('alternar-menu');
    const menuLateral = document.getElementById('menu-lateral');
    const telaPequena = window.matchMedia('(max-width: 991.98px)');
    try {
        if (!telaPequena.matches && localStorage.getItem('recompensa.menuRecolhido') === '1')
            document.body.classList.add('menu-lateral-recolhido');
    } catch { /* O menu continua disponível quando o armazenamento estiver desativado. */ }

    const atualizarMenu = () => {
        const aberto = telaPequena.matches
            ? document.body.classList.contains('menu-lateral-aberto')
            : !document.body.classList.contains('menu-lateral-recolhido');
        botaoMenu?.setAttribute('aria-expanded', String(aberto));
        botaoMenu?.setAttribute('aria-label', aberto ? 'Recolher menu lateral' : 'Expandir menu lateral');
        if (menuLateral) menuLateral.inert = telaPequena.matches && !aberto;
    };

    botaoMenu?.addEventListener('click', () => {
        document.body.classList.toggle(telaPequena.matches ? 'menu-lateral-aberto' : 'menu-lateral-recolhido');
        if (!telaPequena.matches) {
            try {
                localStorage.setItem('recompensa.menuRecolhido', document.body.classList.contains('menu-lateral-recolhido') ? '1' : '0');
            } catch { /* A preferência é opcional; a navegação não depende dela. */ }
        }
        atualizarMenu();
    });
    const fecharMenu = () => {
        document.body.classList.remove('menu-lateral-aberto');
        atualizarMenu();
    };
    document.querySelector('.menu-lateral-sobreposicao')?.addEventListener('click', fecharMenu);
    document.addEventListener('keydown', evento => {
        if (evento.key === 'Escape') { fecharMenu(); botaoMenu?.focus(); }
    });
    telaPequena.addEventListener('change', () => {
        document.body.classList.remove('menu-lateral-aberto', 'menu-lateral-recolhido');
        atualizarMenu();
    });
    atualizarMenu();

    const inicializarPagina = () => {
        document.getElementById('semestre')?.addEventListener('change', evento => evento.target.form.requestSubmit());
        document.querySelectorAll('form[data-confirmacao]').forEach(formulario =>
            formulario.addEventListener('submit', evento => {
                if (!window.confirm(formulario.dataset.confirmacao)) evento.preventDefault();
            }));

        const linhasNotas = document.getElementById('linhas-notas');
        document.getElementById('adicionar-disciplina')?.addEventListener('click', () => {
            if (linhasNotas.children.length >= 30) return;
            const linha = document.createElement('div');
            linha.className = 'nota-linha';
            linha.innerHTML = '<input class="form-control" name="Disciplinas" aria-label="Disciplina" placeholder="Disciplina" required minlength="2" maxlength="100"><input class="form-control nota-valor" name="Valores" aria-label="Nota" placeholder="0,00" inputmode="decimal" required><button type="button" class="icone-botao remover-disciplina" aria-label="Remover disciplina"><i class="fa-solid fa-xmark" aria-hidden="true"></i></button>';
            linhasNotas.appendChild(linha);
            linha.querySelector('input').focus();
        });
        linhasNotas?.addEventListener('click', evento => {
            const botao = evento.target.closest('.remover-disciplina');
            if (botao && linhasNotas.children.length > 1) botao.closest('.nota-linha').remove();
        });

        const grafico = document.getElementById('evolucao-grafico');
        const elementoDados = document.getElementById('dados-grafico');
        if (!grafico || !elementoDados) return;
        const dados = JSON.parse(elementoDados.textContent);
        const espacoNomes = 'http://www.w3.org/2000/svg';
        const desenho = document.createElementNS(espacoNomes, 'svg');
        desenho.setAttribute('viewBox', '0 0 640 180');
        desenho.setAttribute('preserveAspectRatio', 'none');
        desenho.setAttribute('aria-hidden', 'true');

        const desenhar = (tipo, atributos, elementoPai = desenho) => {
            const elemento = document.createElementNS(espacoNomes, tipo);
            Object.entries(atributos).forEach(([chave, valor]) => elemento.setAttribute(chave, valor));
            elementoPai.appendChild(elemento);
            return elemento;
        };
        const valores = dados.series.flatMap(serie => serie.valores).filter(valor => valor !== null);
        const minimo = Math.max(0, Math.floor(Math.min(...valores) - 1));
        const posicaoVertical = valor => 142 - (valor - minimo) / (10 - minimo) * 125;
        const posicaoHorizontal = indice => dados.semestres.length === 1 ? 335 : 48 + indice * 564 / (dados.semestres.length - 1);
        const intervalo = (10 - minimo) > 5 ? 2 : 1;
        for (let valor = minimo; valor <= 10; valor += intervalo) {
            desenhar('line', { x1: 43, y1: posicaoVertical(valor), x2: 617, y2: posicaoVertical(valor), stroke: '#eef1f6', 'stroke-dasharray': '4 5' });
            desenhar('text', { x: 29, y: posicaoVertical(valor) + 3, fill: '#a0aabc', 'font-size': 9, 'text-anchor': 'end' })
                .textContent = valor.toFixed(1).replace('.', ',');
        }
        dados.semestres.forEach((semestre, indice) => {
            if (dados.semestres.length > 8 && indice % Math.ceil(dados.semestres.length / 8) !== 0 && indice !== dados.semestres.length - 1) return;
            desenhar('text', { x: posicaoHorizontal(indice), y: 169, fill: '#939fb2', 'font-size': 9, 'text-anchor': 'middle' })
                .textContent = semestre.descricao;
        });
        const cores = ['#6385bc', '#b4a0b8', '#7fa791', '#bdab7b'];
        dados.series.forEach((serie, indice) => {
            const cor = cores[indice % cores.length];
            let segmento = [];
            const finalizarSegmento = () => {
                if (segmento.length > 1)
                    desenhar('polyline', { points: segmento.join(' '), fill: 'none', stroke: cor, 'stroke-width': 2.3, 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'vector-effect': 'non-scaling-stroke' });
                segmento = [];
            };
            serie.valores.forEach((valor, posicao) => {
                if (valor === null) { finalizarSegmento(); return; }
                segmento.push(posicaoHorizontal(posicao) + ',' + posicaoVertical(valor));
            });
            finalizarSegmento();
            serie.valores.forEach((valor, posicao) => {
                if (valor === null) return;
                const circulo = desenhar('circle', { cx: posicaoHorizontal(posicao), cy: posicaoVertical(valor), r: 3.7, fill: cor, stroke: '#fff', 'stroke-width': 2 });
                desenhar('title', {}, circulo).textContent = serie.nome + ' • ' + dados.semestres[posicao].descricao + ': ' + valor.toFixed(2).replace('.', ',');
            });
        });
        grafico.replaceChildren(desenho);
    };
    document.addEventListener('app:navigated', () => {
        fecharMenu();
        inicializarPagina();
    });
    inicializarPagina();
})();

