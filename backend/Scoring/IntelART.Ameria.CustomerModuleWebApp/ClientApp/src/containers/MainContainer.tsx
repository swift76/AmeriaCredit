import React, { Suspense } from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import AppRoutes from 'routes';
import QueryCachProvider from 'providers/QueryCachProvider';
import UserProvider from 'providers/UserProvider';
import Header from 'components/Header';
import Sidebar from 'components/Sidebar';
import Loading from 'components/Loading';

export default function Main() {
    return (
        <QueryCachProvider>
            <UserProvider>
                <main className="main-layout">
                    <Header />
                    <Container className="page-wrapper" fluid="xl">
                        <Row>
                            <Col lg={3} md={3} sm={12}>
                                <Sidebar />
                            </Col>
                            <Col lg={9} md={9} sm={12}>
                                <Suspense fallback={<Loading className="page-loading" />}>
                                    <AppRoutes />
                                </Suspense>
                            </Col>
                        </Row>
                    </Container>
                </main>
            </UserProvider>
        </QueryCachProvider>
    );
}
