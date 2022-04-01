import React, { Component } from 'react';

export class News extends Component {
    constructor(props) {
        super(props);
        this.state = { loadingNews: "", news: [] };
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleNewsByTitleFragment = this.handleNewsByTitleFragment.bind(this);
        this.getNews = this.getNews.bind(this);
        this.loadNews = this.loadNews.bind(this);
        this.rss = React.createRef();
        this.titleFragment = React.createRef();
    }

    render() {
        let loadingNews = <p><em>{this.state.loadingNews}</em></p>;
        let contents = News.renderNewsTable(this.state.news);
        return (
            <div >
                <h1 id="tabelLabel" >Новости</h1>
                <form onSubmit={this.handleSubmit}>
                    RSS лента:
                    <input type="text" ref={this.rss} defaultValue="https://lenta.ru/rss/news" />
                    <br />
                    <input type="submit" value="Загрузить все новости в базу из ленты" />
                </form>
                {loadingNews}
                <button onClick={this.getNews}>Загрузить все новости из базы</button>
                <form onSubmit={this.handleNewsByTitleFragment}>
                    Фрагмент заголовка:
                    <input type="text" ref={this.titleFragment} defaultValue="" />
                    <br />
                    <input type="submit" value="Загрузить все новости из базы по фрагменту заголовка" />
                </form>
                {contents}
            </div>
        );
    }

    static renderNewsTable(news) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Дата публикации</th>
                        <th>Заголовок</th>
                        <th>Описание</th>
                        <th>Источник</th>
                    </tr>
                </thead>
                <tbody>
                    {news.map(n =>
                        <tr key={n.id}>
                            <td>{n.publishDate}</td>
                            <td>{n.title}</td>
                            <td>{n.summary}</td>
                            <td>{n.source}</td>
                        </tr>
                    )}
                </tbody>
            </table>
            )
    }

    async getNews(event) {
        event.preventDefault();
        const response = await fetch('news', {
            method: 'GET'
        });
        const data = await response.json();
        this.setState({ news: data, loading: false });
    }

    async handleSubmit(event) {
        event.preventDefault();
        const rss = this.rss.current.value;
        this.loadNews(rss);
        this.setState({ loadingNews: "Загрузка ..."});
    }

    async loadNews(rss) {
        const response = await fetch('news', {
            method: 'POST',
            headers:
            {
                'Accept': 'application/json; charset=utf-8',
                'Content-Type': 'application/json;charset=utf-8',
            },
            body: JSON.stringify({ Rss: rss }),
        });
        const data = await response.json();
        if (data.exception != undefined) {
            this.setState({ loadingNews: data.exception });
        }
        else {
            this.setState({ loadingNews: "Загружено новых новостей: " + data});
        }
    }

    async handleNewsByTitleFragment(event) {
        event.preventDefault();
        const titleFragment = this.titleFragment.current.value;
        const response = await fetch('news/' + titleFragment, {
            method: 'GET'
        });
        const data = await response.json();
        this.setState({ news: data });
    }
}
