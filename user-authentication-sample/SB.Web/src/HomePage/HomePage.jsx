import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';

import { userActions } from '../_actions';

class HomePage extends React.Component {
    componentDidMount() {
        this.props.dispatch(userActions.getAll());
    }

    handleDeleteUser(id) {
        return (e) => this.props.dispatch(userActions.delete(id));
    }

    render() {
        const { user, users } = this.props;
        return (
            <div className="col-md-6 col-md-offset-3">
                <h1>Merhaba {user.firstName}!</h1>
                <p>Giriş Yapıldı.</p>
                <h3>Giriş Yapan Kullanıcılar:</h3>
                {users.loading && <em>Kullanıcılar yükleniyor...</em>}
                {users.error && <span className="text-danger">Hata: {users.error}</span>}
                {users.items &&
                    <ul>
                        {users.items.map((user, index) =>
                            <li key={user.id}>
                                {user.firstName + ' ' + user.lastName}
                                {
                                    user.deleting ? <em> - Siliniyor...</em>
                                    : user.deleteError ? <span className="text-danger"> - Hata: {user.deleteError}</span>
                                    : <span> - <a onClick={this.handleDeleteUser(user.id)}>Sil</a></span>
                                }
                            </li>
                        )}
                    </ul>
                }
                <p>
                    <Link to="/login">Çıkış Yap</Link>
                </p>
            </div>
        );
    }
}

function mapStateToProps(state) {
    const { users, authentication } = state;
    const { user } = authentication;
    return {
        user,
        users
    };
}

const connectedHomePage = connect(mapStateToProps)(HomePage);
export { connectedHomePage as HomePage };